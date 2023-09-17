using System;
using System.Collections.Generic;
using DG.Tweening;
using DLBASE;
using FairyGUI;
using UnityEngine;

namespace DLAM
{
    public class PropAnimationManager : DLSingleton<PropAnimationManager>
    {
        public int MaxCount = 20;
        private Dictionary<string, List<GComponent>> showList = new Dictionary<string, List<GComponent>>();
        private Dictionary<string, List<GComponent>> unuseList = new Dictionary<string, List<GComponent>>();

        public void ShowFlyEffect(GObject endNode, string showIconName, int rewardVal, Vector2 xy, float scale = 1)
        {
            int count = Mathf.Min(rewardVal, MaxCount);
            if (xy == Vector2.zero)
            {
                xy.x = GRoot.inst.width * 0.5f;
                xy.y = GRoot.inst.height * 0.5f;
            }

            ShowFlyAni(GRoot.inst, endNode, showIconName, count, xy.x, xy.y, scale, OnFlyAnimationFinish);
        }

        private void OnFlyAnimationFinish(string showIconName, GComponent node)
        {
            // SVibrationManager.ShortVibrateLimit("show_fly_ani_finish", 0.15f);
            // SAudioManager.PlayUISoundLimit(_config.GetFlyFinishSound(animationType), 0.15f, 1);

            showList[showIconName].Remove(node);
            unuseList[showIconName].Add(node);
        }

        private void ShowFlyAni(GComponent parent, GObject endNode, string showIconName, int count, float startX,
            float startY, float scale, Action<String, GComponent> onFinish)
        {
            for (var i = 0; i < count; i++)
            {
                var delay = 0.1f + i * 0.01f;
                parent.RunTween(tw.Callback(0.05f + 0.01f * i, () =>
                {
                    CreateFlyCoin(parent, endNode, showIconName, delay, startX, startY, scale, onFinish);
                }));
            }
        }

        private void CreateFlyCoin(GComponent parent, GObject endNode, string showIconName, float delay, float startX,
            float startY, float scale, Action<String, GComponent> onFinish)
        {
            if (parent == null || endNode == null || showIconName == null)
                return;
            if (parent.isDisposed) return;
            if (endNode.isDisposed) return;

            var node = CreateNewNode(parent, showIconName);
            if (node == null) return;
            node.scale = Vector2.one * scale;
            node.SetXY(startX, startY);
            var endPos = endNode.ConvertToTargetXY(parent);
            var randAngle = RandomUtil.FromTo(0, 18) * 20 * Mathf.Deg2Rad;
            var jx = startX + Mathf.Cos(randAngle) * RandomUtil.FromTo(8, 25) * 10;
            var jy = startY + Mathf.Sin(randAngle) * RandomUtil.FromTo(8, 25) * 6;

            var action1 = tw.MoveTo(0.2F + delay, jx + RandomUtil.FromTo(-10, 10), jy + RandomUtil.FromTo(-2, 4),
                Ease.OutQuart);
            var action2 = tw.MoveTo(0.8f, endPos.x, endPos.y, Ease.OutQuart);
            var action3 = tw.Callback(() =>
            {
                if (node.isDisposed) return;
                onFinish?.Invoke(showIconName, node);
                node.RunTween(tw.AlphaTo(0.2f, 0));
                node.RunTween(tw.ScaleTo(0.3f, 0.5f));

            });
            node.RunTween(tw.Sequence(action1, action2, action3));
        }

        GComponent CreateNewNode(GComponent parent, string showIconName)
        {
            if (parent.isDisposed) return null;
            GComponent node;
            if (!unuseList.ContainsKey(showIconName))
                unuseList.Add(showIconName, new List<GComponent>());
            if (!showList.ContainsKey(showIconName))
                showList.Add(showIconName, new List<GComponent>());
            if (unuseList[showIconName].Count > 0)
            {
                node = unuseList[showIconName][0];
                if (node.isDisposed) return null;
                unuseList[showIconName].RemoveAt(0);
            }
            else
            {
                node = new GComponent {touchable = false, focusable = false, opaque = false};
                parent.AddChild(node);
                var sprite = UIPackage.CreateObjectFromURL(showIconName);
                node.AddChild(sprite);
                sprite.SetPivot(0.5f, 0.5f, true);
            }

            node.alpha = 1;
            node.SetScale(1, 1);
            showList[showIconName].Add(node);
            return node;
        }
    }
}