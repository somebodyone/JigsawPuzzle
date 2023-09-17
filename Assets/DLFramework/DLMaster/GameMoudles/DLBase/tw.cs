using System;
using System.Collections.Generic;
using currency;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using FairyGUI;
using UnityEngine;

namespace Master
{
    public static class tw
    {
        public abstract class TWAction
        {
            protected float _duration = 0;
            protected Ease _ease = Ease.Linear;
            protected int _loop = 0;
            protected float _delay = 0;

            public abstract Tween ApplyTween(GObject target);

            public virtual Tween ApplyTween(Rigidbody target)
            {
                return DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
            }
            public abstract Tween ApplyTween(Transform target);

            public virtual void ApplySequence(Sequence seq, Tween tween)
            {
                seq.Append(tween);
            }

            public TWAction SetLoop(int loop)
            {
                _loop = loop;
                return this;
            }

            public TWAction SetDelay(float delay)
            {
                _delay = delay;
                return this;
            }
            
            public TWAction SetEase(Ease ease)
            {
                _ease = ease;
                return this;
            }            
        }

        private class TWMoveTo : TWAction
        {
            protected readonly Vector3 _position;
            protected readonly bool _isMoveZ;

            internal TWMoveTo(float duration, float x, float y, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _position.x = x;
                _position.y = y;
                _position.z = 0;
            }
            
            internal TWMoveTo(float duration, float x, float y, float z, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _position.x = x;
                _position.y = y;
                _position.z = z;
                _isMoveZ = true;
            }

            public override Tween ApplyTween(GObject target)
            {
                Vector3 Getter() => target.position;
                void Setter(Vector3 pos)
                {
                    if(!target.isDisposed) target.position = pos;
                }

                var tweenCore = DOTween.To(Getter, Setter, _position, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }

            public override Tween ApplyTween(Transform target)
            {
                Vector3 Getter() => target == null?Vector3.zero:target.localPosition;
                void Setter(Vector3 pos)
                {
                    if (target != null)
                    {
                        target.localPosition = _isMoveZ ? pos : new Vector3(pos.x, pos.y, target.localPosition.z);
                    }
                }

                var tweenCore = DOTween.To(Getter, Setter, _position, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }
            
            public override Tween ApplyTween(Rigidbody target)
            {
                Vector3 Getter() => target.position;
                void Setter(Vector3 pos) => target.MovePosition(_isMoveZ?pos:new Vector3(pos.x, pos.y, target.position.z));
                var tweenCore = DOTween.To(Getter, Setter, _position, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }
        }

        private class TWMoveBy : TWMoveTo
        {
            internal TWMoveBy(float duration, float x, float y, Ease ease) : base(duration, x, y, ease) { }
            internal TWMoveBy(float duration, float x, float y, float z, Ease ease) : base(duration, x, y, z, ease) { }
            public override Tween ApplyTween(GObject target) { return base.ApplyTween(target).SetRelative(true); }
            public override Tween ApplyTween(Transform target) { return base.ApplyTween(target).SetRelative(true); }
            public override Tween ApplyTween(Rigidbody target) { return base.ApplyTween(target).SetRelative(true); }
        }
        
        private class TWMoveAxisTo : TWAction
        {
            protected readonly float _value;
            protected readonly AxisConstraint _axis = AxisConstraint.X;
            internal TWMoveAxisTo(float duration, float v, AxisConstraint axis, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _value = v;
                _axis = axis;
            }
            public override Tween ApplyTween(GObject target)
            {
                var endValue = Vector3.zero;
                switch (_axis)
                {
                    case AxisConstraint.X: endValue.x = _value; break;
                    case AxisConstraint.Y: endValue.y = _value; break;
                    case AxisConstraint.Z: endValue.z = _value; break;
                }
                var tweenCore = DOTween.To(() => target.position, pos=>
                {
                    if(!target.isDisposed) target.position = pos;
                }, endValue , _duration);
                return tweenCore.SetOptions(_axis).SetTarget(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
            }
            public override Tween ApplyTween(Transform target)
            {
                var endValue = Vector3.zero;
                switch (_axis)
                {
                    case AxisConstraint.X: endValue.x = _value; break;
                    case AxisConstraint.Y: endValue.y = _value; break;
                    case AxisConstraint.Z: endValue.z = _value; break;
                }
                var tweenCore = DOTween.To(() => target==null?Vector3.zero:target.localPosition, pos=>
                {
                    if (target != null) target.localPosition = pos;
                },  endValue, _duration);
                return tweenCore.SetOptions(_axis).SetTarget(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
            }
            
            public override Tween ApplyTween(Rigidbody target)
            {
                var endValue = Vector3.zero;
                switch (_axis)
                {
                    case AxisConstraint.X: endValue.x = _value; break;
                    case AxisConstraint.Y: endValue.y = _value; break;
                    case AxisConstraint.Z: endValue.z = _value; break;
                }
                var tweenCore = DOTween.To(() => target==null?Vector3.zero:target.position, target.MovePosition,  endValue, _duration);
                return tweenCore.SetOptions(_axis).SetTarget(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
            }
        }

        private class TWMoveAxisBy : TWMoveAxisTo
        {
            internal TWMoveAxisBy(float duration, float v, AxisConstraint axis, Ease ease) : base(duration, v, axis, ease) { }
            public override Tween ApplyTween(GObject target) { return base.ApplyTween(target).SetRelative(true); }
            public override Tween ApplyTween(Transform target) { return base.ApplyTween(target).SetRelative(true); }
        }

        private class TWScaleTo : TWAction
        {
            protected readonly Vector3 _scale;
            protected readonly bool isScaleZ = false;

            internal TWScaleTo(float duration, float x, float y, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _scale.x = x;
                _scale.y = y;
            }
            
            internal TWScaleTo(float duration, float x, float y, float z, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _scale.x = x;
                _scale.y = y;
                _scale.z = z;
                isScaleZ = true;
            }

            public override Tween ApplyTween(GObject target)
            {
                Vector2 Getter() => target.scale;
                void Setter(Vector2 v)
                {
                    if(!target.isDisposed)target.scale = v;
                }

                var tweenCore = DOTween.To(Getter, Setter, _scale, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }

            public override Tween ApplyTween(Transform target)
            {
                Vector3 Getter() => target==null?Vector3.zero:target.localScale;
                void Setter(Vector3 v)
                {
                    if (target != null) target.localScale = isScaleZ ? v : new Vector3(v.x, v.y, target.localScale.z);
                }

                var tweenCore = DOTween.To(Getter, Setter, _scale, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }
        }

        private class TWScaleBy : TWScaleTo
        {
            internal TWScaleBy(float duration, float x, float y, Ease ease) : base(duration, x, y, ease)
            {
            }
            internal TWScaleBy(float duration, float x, float y, float z, Ease ease) : base(duration, x, y, z, ease)
            {
            }

            public override Tween ApplyTween(GObject target)
            {
                return base.ApplyTween(target).SetRelative(true);
            }
            public override Tween ApplyTween(Transform target)
            {
                return base.ApplyTween(target).SetRelative(true);
            }
        }

        private class TWShake : TWAction
        {
            private readonly float _strength = 3;
            private int _vibrato = 10;
            private float _randomness = 90;

            internal TWShake(float duration, float strength, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _strength = strength;
            }

            public override Tween ApplyTween(GObject target)
            {
                Vector3 Getter() => target.position;
                void Setter(Vector3 v)
                {
                    if(!target.isDisposed) target.position = v;
                }
                var tweenCore = DOTween.Shake(Getter, Setter, _duration, _strength, _vibrato, _randomness, true, true)
                                       .SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake)
                                       .SetOptions(false);
                tweenCore.SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }

            public override Tween ApplyTween(Transform target)
            {
                Vector3 Getter() => target == null?Vector3.zero:target.localPosition;
                void Setter(Vector3 v)
                {
                    if (target != null) target.localPosition = v;
                }

                var tweenCore = DOTween.Shake(Getter, Setter, _duration, _strength, _vibrato, _randomness, false, true)
                                       .SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake)
                                       .SetOptions(false);
                tweenCore.SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }
        }

        public enum NumberMode
        {
            Normal,
            KMBT,
            Currency,
            Thousand,
            ThousandAtom //千分位显示，传入参数为原子级，显示真实数量
        }
        private class TWTextIntFromTo : TWAction
        {
            private readonly long _fromV;
            private readonly long _toV;
            private string _prefix;
            private string _suffix;
            private NumberMode _numberMode = 0;//1使用kmbt，2除以10000转成3位小数

            internal TWTextIntFromTo(float duration, long fromValue, long toValue, NumberMode numberMode, string prefix, string suffix, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _fromV = fromValue;
                _toV = toValue;
                _prefix = prefix;
                _suffix = suffix;
                _numberMode = numberMode;
            }

            public override Tween ApplyTween(GObject target)
            {
                long Getter() => _fromV;

                void Setter(long v)
                {
                    var str = "";
                    switch (_numberMode)
                    {
                        case NumberMode.Normal: str = v.ToString(); break;
                        case NumberMode.KMBT: str = Util.ToKMBTUnitString(v); break;
                        case NumberMode.Currency: str = Currency.ToRateMoneyString(v * 0.0001, !Currency.GetSymbol().Equals(CurrencyUSD.GetSymbol())); break;
                        case NumberMode.Thousand: str = Util.ToThousandsString(v); break;
                        case NumberMode.ThousandAtom: str = Util.ToThousandAtomString(v); break;
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = _prefix + str;
                    }

                    if (!string.IsNullOrEmpty(str))
                    {
                        str += _suffix;
                    }

                    if(!target.isDisposed) target.text = str;
                }

                var tweenCore = DOTween.To(Getter, Setter, _toV, _duration);
                tweenCore.SetTarget(target).SetDelay(_delay).SetEase(_ease).SetLoops(_loop);
                return tweenCore;
            }

            public override Tween ApplyTween(Transform target)
            {
                return null;
            }
        }
        
        
        private class TWNumberFromTo : TWAction
        {
            private readonly float _fromV;
            private readonly float _toV;
            private readonly DOSetter<float> _stepCallback;

            internal TWNumberFromTo(float duration, float fromValue, float toValue, DOSetter<float> stepCallback,Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _fromV = fromValue;
                _toV = toValue;
                _stepCallback = stepCallback;
            }

            public override Tween ApplyTween(GObject target)
            {
                float Getter() => _fromV;
                var tweenCore = DOTween.To(Getter, _stepCallback, _toV, _duration);
                tweenCore.SetTarget(target).SetDelay(_delay).SetEase(_ease).SetLoops(_loop);
                return tweenCore;
            }

            public override Tween ApplyTween(Transform target)
            {
                float Getter() => _fromV;
                var tweenCore = DOTween.To(Getter, _stepCallback, _toV, _duration);
                tweenCore.SetTarget(target).SetDelay(_delay).SetEase(_ease).SetLoops(_loop);
                return tweenCore;
            }
        }

        private class TWTextTemplateValueFromTo : TWAction
        {
            private readonly long _fromV;
            private readonly long _toV;
            private readonly string _key;

            internal TWTextTemplateValueFromTo(float duration, long fromValue, long toValue, string key, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _fromV = fromValue;
                _toV = toValue;
                _key = key;
            }

            public override Tween ApplyTween(GObject target)
            {
                long Getter() => _fromV;
                var textField = target.asTextField;
                void Setter(long v)
                {
                    if(!target.isDisposed) textField.SetVar(_key, v.ToString()).FlushVars();
                }

                var tweenCore = DOTween.To(Getter, Setter, _toV, _duration);
                tweenCore.SetTarget(target).SetDelay(_delay).SetEase(_ease).SetLoops(_loop);
                return tweenCore;
            }

            public override Tween ApplyTween(Transform target)
            {
                return null;
            }
        }

        
        private class TWSequence : TWAction
        {
            private readonly TWAction[] _actionList;

            public TWSequence(TWAction[] args)
            {
                _actionList = args;
            }

            public override Tween ApplyTween(GObject target)
            {
                var seq = DOTween.Sequence();
                foreach (var action in _actionList)
                {
                    var t = action.ApplyTween(target);
                    action.ApplySequence(seq, t);
                }

                seq.SetDelay(_delay).SetLoops(_loop).SetTarget(target);
                return seq;
            }

            public override Tween ApplyTween(Transform target)
            {
                var seq = DOTween.Sequence();
                foreach (var action in _actionList)
                {
                    var t = action.ApplyTween(target);
                    action.ApplySequence(seq, t);
                }

                seq.SetDelay(_delay).SetLoops(_loop).SetTarget(target);
                return seq;
            }
        }

        private static readonly DOGetter<float> s_emptyFloatGetter = () => 0;
        private static readonly DOSetter<float> s_emptyFloatSetter = v => { };

        private class TWRemoveSelf : TWAction
        {
            internal TWRemoveSelf(float duration)
            {
                _duration = duration;
            }

            public override Tween ApplyTween(GObject target)
            {
                Tween t = DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
                t.onComplete = target.Dispose;
                t.SetTarget(target).SetDelay(_delay);
                return t;
            }

            public override Tween ApplyTween(Transform target)
            {
                Tween t = DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
                t.onComplete = () => { GameObject.Destroy(target.gameObject); };
                t.SetTarget(target).SetDelay(_delay);
                return t;
            }
            
            public override Tween ApplyTween(Rigidbody target)
            {
                Tween t = DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
                t.onComplete = () => { GameObject.Destroy(target.gameObject); };
                t.SetTarget(target).SetDelay(_delay);
                return t;
            }
        }

        private class TWCallback : TWAction
        {
            private TweenCallback _callback;

            internal TWCallback(float duration, TweenCallback callback)
            {
                _duration = duration;
                _callback = callback;
            }

            public override Tween ApplyTween(GObject target)
            {
                Tween t = DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
                t.onComplete = _callback;
                t.SetTarget(target).SetDelay(_delay);
                return t;
            }

            public override Tween ApplyTween(Transform target)
            {
                Tween t = DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
                t.onComplete = _callback;
                t.SetTarget(target).SetDelay(_delay);
                return t;
            }
            
            public override Tween ApplyTween(Rigidbody target)
            {
                Tween t = DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
                t.onComplete = _callback;
                t.SetTarget(target).SetDelay(_delay);
                return t;
            }
        }
        private class TWSeqCallback : TWAction
        {
            private TweenCallback _callback;

            internal TWSeqCallback(float delay, TweenCallback callback)
            {
                _duration = delay;
                _callback = callback;
            }

            public override Tween ApplyTween(GObject target)
            {
                return null;
            }

            public override Tween ApplyTween(Transform target)
            {
                return null;
            }
            
            public override Tween ApplyTween(Rigidbody target)
            {
                return null;
            }

            public override void ApplySequence(Sequence seq, Tween tween)
            {
                if (_duration > 0)
                {
                    seq.AppendInterval(_duration);
                }
                seq.AppendCallback(_callback);
            }
        }
        private class TWDelay : TWAction
        {
            internal TWDelay(float duration)
            {
                _duration = duration;
            }

            public override Tween ApplyTween(GObject target)
            {
                return DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
            }

            public override Tween ApplyTween(Transform target)
            {
                return DOTween.To(s_emptyFloatGetter, s_emptyFloatSetter, 1f, _duration);
            }

            public override void ApplySequence(Sequence seq, Tween tween)
            {
                seq.AppendInterval(_duration);
            }
        }


        private class TWAlphaTo : TWAction
        {
            private readonly float _alpha;

            internal TWAlphaTo(float duration, float alpha)
            {
                _duration = duration;
                _alpha = alpha;
            }

            public override Tween ApplyTween(GObject target)
            {
                Tween t = DOTween.To(() => target.alpha, v =>
                {
                    if(!target.isDisposed)target.alpha = v;
                }, _alpha, _duration);
                t.SetTarget(target).SetDelay(_delay).SetEase(_ease).SetLoops(_loop);
                return t;
            }

            public override Tween ApplyTween(Transform target)
            {
                Tween t = DOTween.To(() => 0, v => { }, _alpha, _duration);
                t.SetTarget(target).SetDelay(_delay).SetEase(_ease).SetLoops(_loop);
                return t;
            }
        }


        private class TWBezierFromTo : TWAction
        {
            private readonly double _endY;
            private readonly double _endX;
            private readonly double _startY;
            private readonly double _startX;
            private readonly double _control1X;
            private readonly double _control1Y;
            private readonly double _control2X;
            private readonly double _control2Y;

            internal TWBezierFromTo(float duration, float fromX, float fromY, float tox, float toy, float length1,
                                    float height1, float length2, float height2, int dir)
            {
                _duration = duration;
                _startX = fromX;
                _startY = fromY;
                _endX = tox;
                _endY = toy;
                var dx = tox - fromX;
                var dy = toy - fromY;
                var cx = dx * length1 + fromX;
                var cy = dy * length1 + fromY;
                if (dir >= 0)
                {
                    cx += -dy * height1;
                    cy += dx * height1;
                }
                else
                {
                    cx += dy * height1;
                    cy += -dx * height1;
                }

                _control1X = cx;
                _control1Y = cy;

                cx = dx * length2 + fromX;
                cy = dy * length2 + fromY;
                if (dir >= 0)
                {
                    cx += -dy * height2;
                    cy += dx * height2;
                }
                else
                {
                    cx += dy * height2;
                    cy += -dx * height2;
                }

                _control2X = cx;
                _control2Y = cy;
            }

            private static double BezierRat(double a, double b, double c, double d, double t)
            {
                return Math.Pow(1 - t, 3) * a + 3 * t * (Math.Pow(1 - t, 2)) * b + 3 * Math.Pow(t, 2) * (1 - t) * c +
                       Math.Pow(t, 3) * d;
            }

            public override Tween ApplyTween(GObject target)
            {
                void Setter(float ratio)
                {
                    var x = BezierRat(_startX, _control1X, _control2X, _endX, ratio);
                    var y = BezierRat(_startY, _control1Y, _control2Y, _endY, ratio);
                    if(!target.isDisposed)
                    {
                        target.x = (float) x;
                        target.y = (float) y;
                    }
                }

                Tween t = DOTween.To(() => 0f, Setter, 1f, _duration);
                t.SetTarget(target).SetDelay(_delay).SetEase(_ease);

                return t;
            }

            public override Tween ApplyTween(Transform target)
            {
                return null;
            }
        }
        
        private class TWBezierFromToXZY : TWAction
        {
            private readonly double _endY;
            private readonly double _endX;
            private readonly double _endZ;
            private readonly double _startY;
            private readonly double _startX;
            private readonly double _startZ;
            private readonly double _control1X;
            private readonly double _control1Y;
            private readonly double _control1Z;
            private readonly double _control2X;
            private readonly double _control2Y;
            private readonly double _control2Z;

            internal TWBezierFromToXZY(float duration, float fromX, float fromY, float fromZ, float tox, float toy, float toz, float length1,
                                       float height1, float length2, float height2, int dir)
            {
                _duration = duration;
                _startX = fromX;
                _startY = fromY;
                _startZ = fromZ;
                _endX = tox;
                _endY = toy;
                _endZ = toz;
                var startPos = new Vector3(fromX, fromY, fromZ);
                var endPos = new Vector3(tox, toy, toz);
                var v = endPos - startPos;
                var length = v.magnitude;
                var nvA = Vector3.zero;
                var nvB = Vector3.zero;
                Vector3.OrthoNormalize(ref v, ref nvA, ref nvB);
                var c1 = length1 * length * v + startPos;
                var c2 = length2 * length * v + startPos;
                if (dir > 0)
                {
                    c1 += -height1 * length * nvA;
                    c2 += -height2 * length * nvA;
                }
                else
                {
                    c1 += height1 * length * nvA;
                    c2 += height2 * length * nvA;
                }
                _control1X = c1.x;
                _control1Y = c1.y;
                _control1Z = c1.z;
                
                _control2X = c2.x;
                _control2Y = c2.y;
                _control2Z = c2.z;
            }

            private static double BezierRat(double a, double b, double c, double d, double t)
            {
                return Math.Pow(1 - t, 3) * a + 3 * t * (Math.Pow(1 - t, 2)) * b + 3 * Math.Pow(t, 2) * (1 - t) * c +
                       Math.Pow(t, 3) * d;
            }

            public override Tween ApplyTween(Transform target)
            {
                void Setter(float ratio)
                {
                    if (target == null)
                    {
                        return;
                    }
                    var x = BezierRat(_startX, _control1X, _control2X, _endX, ratio);
                    var y = BezierRat(_startY, _control1Y, _control2Y, _endY, ratio);
                    var z = BezierRat(_startZ, _control1Z, _control2Z, _endZ, ratio);
                    target.transform.localPosition = new Vector3((float)x, (float)y, (float)z);
                }
                Tween t = DOTween.To(() => 0f, Setter, 1f, _duration);
                t.SetTarget(target).SetDelay(_delay).SetEase(_ease);

                return t;
            }

            public override Tween ApplyTween(GObject target)
            {
                return null;
            }
        }

        private class TWPath : TWAction
        {
            private Vector3[] _path;
            private PathType _pathType;
            private PathMode _pathMode;
            private int _resolution;
            private Color? _gizmoColor;

            internal TWPath(float duration, Vector3[] path, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
            {
                _path = path;
                _duration = duration;
                _pathType = pathType;
                _pathMode = pathMode;
                if (resolution < 1) resolution = 1;
                _resolution = resolution;
                _gizmoColor = gizmoColor;
            }

            public override Tween ApplyTween(Transform target)
            {
                var t = DOTween.To(PathPlugin.Get(), () => target == null?Vector3.zero:target.position, x =>
                {
                    if (target != null)target.position = x;
                }, new Path(_pathType, _path, _resolution, _gizmoColor), _duration);
                t.plugOptions.mode = _pathMode;
                t.SetTarget(target).SetDelay(_delay).SetEase(_ease).SetLoops(_loop);
                return t;
            }

            public override Tween ApplyTween(GObject target)
            {
                var t = DOTween.To(PathPlugin.Get(), () => target.position, x =>
                {
                    if(!target.isDisposed)target.position = x;
                }, new Path(_pathType, _path, _resolution, _gizmoColor), _duration);
                t.plugOptions.mode = _pathMode;
                t.SetTarget(target.displayObject.gameObject.transform).SetDelay(_delay).SetEase(_ease).SetLoops(_loop);
                return t;
            }
        }
        
        private class TWRotateBy : TWRotateTo
        {
            internal TWRotateBy(float duration, float rotation, Ease ease) : base(duration, rotation, ease)
            {
            }

            public override Tween ApplyTween(GObject target)
            {
                float Getter() => target.rotation;
                void Setter(float r)
                {
                    if(!target.isDisposed) target.rotation = r;
                }

                var tweenCore = DOTween.To(Getter, Setter, _rotation, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay).SetRelative(true);
                return tweenCore;
            }
        }
        
        private class TWRotateTo : TWAction
        {
            protected readonly float _rotation = 0;

            internal TWRotateTo(float duration, float rotation, Ease ease)
            {
                _duration = duration;
                _ease = ease;
                _rotation = rotation;
            }

            public override Tween ApplyTween(GObject target)
            {
                float Getter() => target.rotation;
                void Setter(float r)
                {
                    if(!target.isDisposed)target.rotation = r;
                }
                var tweenCore = DOTween.To(Getter, Setter, _rotation, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }

            public override Tween ApplyTween(Transform target)
            {
                return null;
            }
        }
        
        private class TWRotateXTo : TWRotateTo
        {
            internal TWRotateXTo(float duration, float rotation, Ease ease) : base(duration, rotation, ease)
            {
            }
            public override Tween ApplyTween(Transform target)
            {
                float Getter() => target == null?0:target.localRotation.eulerAngles.x;
                void Setter(float r)
                {
                    if (target != null)
                    {
                        var eulerAngles = target.localRotation.eulerAngles;
                        eulerAngles.x = r;
                        target.localRotation = Quaternion.Euler(eulerAngles);
                    }
                }

                var tweenCore = DOTween.To(Getter, Setter, _rotation, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }
        }
        
        private class TWRotateYTo : TWRotateTo
        {
            internal TWRotateYTo(float duration, float rotation, Ease ease) : base(duration, rotation, ease)
            {
            }
            public override Tween ApplyTween(Transform target)
            {
                float Getter() => target == null?0:target.localRotation.eulerAngles.y;
                void Setter(float r)
                {
                    if (target != null)
                    {
                        var eulerAngles = target.localRotation.eulerAngles;
                        eulerAngles.y = r;
                        target.localRotation = Quaternion.Euler(eulerAngles);
                    }
                }

                var tweenCore = DOTween.To(Getter, Setter, _rotation, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }
        }

        
        private class TWRotateZTo : TWRotateTo
        {
            internal TWRotateZTo(float duration, float rotation, Ease ease) : base(duration, rotation, ease)
            {
            }
            public override Tween ApplyTween(Transform target)
            {
                float Getter() => target == null?0:target.localRotation.eulerAngles.z;
                void Setter(float r)
                {
                    if (target != null)
                    {
                        var eulerAngles = target.localRotation.eulerAngles;
                        eulerAngles.z = r;
                        target.localRotation = Quaternion.Euler(eulerAngles);
                    }
                }

                var tweenCore = DOTween.To(Getter, Setter, _rotation, _duration);
                tweenCore.SetTarget<Tweener>(target).SetEase(_ease).SetLoops(_loop).SetDelay(_delay);
                return tweenCore;
            }
        }

        private class TWRotateXBy : TWRotateXTo
        {
            internal TWRotateXBy(float duration, float rotation, Ease ease) : base(duration, rotation, ease) { }
            public override Tween ApplyTween(GObject target)
            {
                return base.ApplyTween(target).SetRelative(true);
            }
            public override Tween ApplyTween(Transform target)
            {
                return base.ApplyTween(target).SetRelative(true);
            }
        }
        
        private class TWRotateYBy : TWRotateYTo
        {
            internal TWRotateYBy(float duration, float rotation, Ease ease) : base(duration, rotation, ease) { }
            public override Tween ApplyTween(GObject target)
            {
                return base.ApplyTween(target).SetRelative(true);
            }
            public override Tween ApplyTween(Transform target)
            {
                return base.ApplyTween(target).SetRelative(true);
            }
        }
        
        private class TWRotateZBy : TWRotateYTo
        {
            internal TWRotateZBy(float duration, float rotation, Ease ease) : base(duration, rotation, ease) { }
            public override Tween ApplyTween(GObject target) { return base.ApplyTween(target).SetRelative(true); }
            public override Tween ApplyTween(Transform target) { return base.ApplyTween(target).SetRelative(true); }
        }

        public static TWAction Path(float duration, Vector3[] path, Ease ease = Ease.Linear, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            return new TWPath(duration, path, pathType, pathMode, resolution, gizmoColor).SetEase(ease);
        }
        
        public static TWAction MoveByX(float duration, float x, Ease ease = Ease.Linear)
        {
            return new TWMoveAxisBy(duration, x, AxisConstraint.X, ease);
        }
        public static TWAction MoveToX(float duration, float x, Ease ease = Ease.Linear)
        {
            return new TWMoveAxisTo(duration, x, AxisConstraint.X, ease);
        }
        public static TWAction MoveByY(float duration, float y, Ease ease = Ease.Linear)
        {
            return new TWMoveAxisBy(duration, y, AxisConstraint.Y, ease);
        }
        public static TWAction MoveToY(float duration, float y, Ease ease = Ease.Linear)
        {
            return new TWMoveAxisTo(duration, y, AxisConstraint.Y, ease);
        }
        public static TWAction MoveByZ(float duration, float z, Ease ease = Ease.Linear)
        {
            return new TWMoveAxisBy(duration, z, AxisConstraint.Z, ease);
        }
        public static TWAction MoveToZ(float duration, float z, Ease ease = Ease.Linear)
        {
            return new TWMoveAxisTo(duration, z, AxisConstraint.Z, ease);
        }
        
        public static TWAction MoveTo(float duration, Vector2 pos, Ease ease = Ease.Linear)
        {
            return new TWMoveTo(duration, pos.x, pos.y, ease);
        }

        public static TWAction MoveBy(float duration, Vector2 pos, Ease ease = Ease.Linear)
        {
            return new TWMoveBy(duration, pos.x, pos.y, ease);
        }

        public static TWAction MoveTo(float duration, float x, float y, Ease ease = Ease.Linear)
        {
            return new TWMoveTo(duration, x, y, ease);
        }
        
        public static TWAction MoveTo(float duration, float x, float y, float z, Ease ease = Ease.Linear)
        {
            return new TWMoveTo(duration, x, y, z, ease);
        }

        public static TWAction MoveBy(float duration, float x, float y, Ease ease = Ease.Linear)
        {
            return new TWMoveBy(duration, x, y, ease);
        }
        
        public static TWAction MoveBy(float duration, float x, float y, float z, Ease ease = Ease.Linear)
        {
            return new TWMoveBy(duration, x, y, z, ease);
        }

        public static TWAction AlphaTo(float duration, float alpha, Ease ease = Ease.Linear)
        {
            return new TWAlphaTo(duration, alpha);
        }

        public static TWAction ScaleTo(float duration, float scale, Ease ease = Ease.Linear)
        {
            return new TWScaleTo(duration, scale, scale, scale, ease);
        }
        
        public static TWAction ScaleTo(float duration, float scaleX, float scaleY, Ease ease = Ease.Linear)
        {
            return new TWScaleTo(duration, scaleX, scaleY, 1f, ease);
        }
        
        public static TWAction ScaleTo(float duration, Vector2 scale, Ease ease = Ease.Linear)
        {
            return new TWScaleTo(duration, scale.x, scale.y, 1f, ease);
        }

        public static TWAction ScaleTo(float duration, float scaleX, float scaleY, float scaleZ, Ease ease = Ease.Linear)
        {
            return new TWScaleTo(duration, scaleX, scaleY, scaleZ, ease);
        }
        
        public static TWAction ScaleBy(float duration, float scale, Ease ease = Ease.Linear)
        {
            return new TWScaleBy(duration, scale, scale, scale, ease);
        }

        public static TWAction ScaleBy(float duration, float scaleX, float scaleY, Ease ease = Ease.Linear)
        {
            return new TWScaleBy(duration, scaleX, scaleY, ease);
        }
        
        public static TWAction ScaleBy(float duration, Vector2 scale, Ease ease = Ease.Linear)
        {
            return new TWScaleBy(duration, scale.x, scale.y, ease);
        }
        
        public static TWAction ScaleBy(float duration, float scaleX, float scaleY, float scaleZ, Ease ease = Ease.Linear)
        {
            return new TWScaleBy(duration, scaleX, scaleY, scaleZ, ease);
        }
                
        public static TWAction RotateBy(float duration, float rotation,Ease ease = Ease.Linear)
        {
            return new TWRotateBy(duration, rotation, ease);
        }
        
        public static TWAction RotateXTo(float duration, float rotation,Ease ease = Ease.Linear)
        {
            return new TWRotateXTo(duration, rotation, ease);
        }
        public static TWAction RotateXBy(float duration, float rotation,Ease ease = Ease.Linear)
        {
            return new TWRotateXBy(duration, rotation, ease);
        }
        
        public static TWAction RotateYTo(float duration, float rotation,Ease ease = Ease.Linear)
        {
            return new TWRotateYTo(duration, rotation, ease);
        }
        public static TWAction RotateYBy(float duration, float rotation,Ease ease = Ease.Linear)
        {
            return new TWRotateYBy(duration, rotation, ease);
        }
        
        public static TWAction RotateZTo(float duration, float rotation,Ease ease = Ease.Linear)
        {
            return new TWRotateZTo(duration, rotation, ease);
        }
        public static TWAction RotateZBy(float duration, float rotation,Ease ease = Ease.Linear)
        {
            return new TWRotateZBy(duration, rotation, ease);
        }
        
        public static TWAction RotateTo(float duration, float rotation, Ease ease = Ease.Linear)
        {
            return new TWRotateTo(duration,rotation,ease);
        }        

        public static TWAction Shake(float duration, float strength, Ease ease = Ease.Linear)
        {
            return new TWShake(duration, strength, ease);
        }

        public static TWAction BezierFromTo(float duration, float fromX, float fromY, float tox, float toy,
                                            float length1, float height1, float length2, float height2, int dir)
        {
            return new TWBezierFromTo(duration, fromX, fromY, tox, toy, length1, height1, length2, height2, dir);
        }
        
        public static TWAction BezierFromTo(float duration, Vector3 fromPos, Vector3 toPos,
                                            float length1, float height1, float length2, float height2, int dir)
        {
            return new TWBezierFromToXZY(duration, fromPos.x, fromPos.y, fromPos.z, toPos.x, toPos.y, toPos.z, length1, height1, length2, height2, dir);
        }

        public static TWAction RemoveSelf(float duration = 0)
        {
            return new TWRemoveSelf(duration);
        }

        public static TWAction Delay(float duration = 0)
        {
            return new TWDelay(duration);
        }

        public static TWAction Callback(TweenCallback callback)
        {
            return new TWCallback(0, callback);
        }
        
        public static TWAction SeqCallback(TweenCallback callback)
        {
            return new TWSeqCallback(0, callback);
        }
        
        public static TWAction SeqCallback(float duration, TweenCallback callback)
        {
            return new TWSeqCallback(duration, callback);
        }

        public static TWAction Callback(float duration, TweenCallback callback)
        {
            return new TWCallback(duration, callback);
        }

        public static TWAction TextIntFromTo(float duration, long fromValue, long toValue, NumberMode numberMode, string prefix, string suffix, Ease ease = Ease.Linear)
        {
            return new TWTextIntFromTo(duration, fromValue, toValue, numberMode, prefix, suffix, ease);
        }
        
        public static TWAction TextTemplateValueFromTo(float duration, long fromValue, long toValue, string key, Ease ease = Ease.Linear)
        {
            return new TWTextTemplateValueFromTo(duration, fromValue, toValue, key, ease);
        }
        
        public static TWAction NumberFromTo(float duration, float fromValue, float toValue, DOSetter<float> stepCallback, Ease ease = Ease.Linear)
        {
            return new TWNumberFromTo(duration, fromValue, toValue, stepCallback, ease);
        }
        
        public static TWAction Sequence(params TWAction[] args)
        {
            return new TWSequence(args);
        }


        private static readonly Dictionary<EventDispatcher, List<Tween>> s_allTweenA =
            new Dictionary<EventDispatcher, List<Tween>>();
        
        private static readonly Dictionary<EventDispatcher, List<LinkedListNode<Tween>>> s_allTweenX =
            new Dictionary<EventDispatcher, List<LinkedListNode<Tween>>>();
        
        private static readonly LinkedList<Tween> s_allTweenListX = new LinkedList<Tween>();
        
        private static readonly Dictionary<int, List<LinkedListNode<Tween>>> s_allTweenY =
            new Dictionary<int, List<LinkedListNode<Tween>>>();

        private static readonly Dictionary<int, List<Tween>> s_allTweenB = new Dictionary<int, List<Tween>>();

        public static void ClearAllTween()
        {
            
            foreach (var info in s_allTweenA) info.Value.ForEach(ele =>
            {
                ele.onKill = null;
                ele.Kill();
            });
            s_allTweenA.Clear();
            foreach (var info in s_allTweenB) info.Value.ForEach(ele =>
            {
                ele.onKill = null;
                ele.Kill();
            });
            s_allTweenB.Clear();
            foreach (var info in s_allTweenX) info.Value.ForEach(ele =>
            {
                ele.Value.onKill = null;
                ele.Value.Kill();
            });
            s_allTweenX.Clear();
            foreach (var info in s_allTweenListX)
            {
                info.onKill = null;
                info.Kill();
            }
            s_allTweenListX.Clear();
            foreach (var info in s_allTweenY) info.Value.ForEach(ele =>
            {
                ele.Value.onKill = null;
                ele.Value.Kill();
            });
            s_allTweenY.Clear();
        }

        private static void OnTargetRemovedFromStage(EventContext e)
        {
            if (!s_allTweenA.ContainsKey(e.sender))
            {
                return;
            }

            var tweenList = s_allTweenA[e.sender];
            for (var i = tweenList.Count - 1; i >= 0; i--)
            {
                tweenList[i].onComplete = null;
                tweenList[i].onKill = null;
                tweenList[i].Kill();
            }

            tweenList.Clear();
            s_allTweenA.Remove(e.sender);
        }
        
        private static void OnTargetRemovedFromStageX(EventContext e)
        {
            if (!s_allTweenX.ContainsKey(e.sender))
            {
                return;
            }

            var tweenList = s_allTweenX[e.sender];
            for (var i = tweenList.Count - 1; i >= 0; i--)
            {
                s_allTweenListX.Remove(tweenList[i].Value);
                tweenList[i].Value.onKill = null;
                tweenList[i].Value.Kill();
            }

            tweenList.Clear();
            s_allTweenX.Remove(e.sender);
        }

        public static Tween RunTween(this GObject target, TWAction action)
        {
            if (!s_allTweenA.ContainsKey(target))
            {
                s_allTweenA.Add(target, new List<Tween>());
            }
            
            var tweenList = s_allTweenA[target];
            var t = action.ApplyTween(target);
            tweenList.Add(t);
            target.onDisposed.Add(OnTargetRemovedFromStage);
            t.onKill = () => tweenList.Remove(t);
            return t;
        }
        
        public static Tween RunTweenX(this GObject target, TWAction action)
        {
            if (!s_allTweenX.ContainsKey(target))
            {
                s_allTweenX.Add(target, new List<LinkedListNode<Tween>>());
            }
            
            var tweenList = s_allTweenX[target];
            var t = action.ApplyTween(target);
            t.SetUpdate(UpdateType.Manual);
            var node = s_allTweenListX.AddLast(t);
            tweenList.Add(node);
            
            target.onDisposed.Add(OnTargetRemovedFromStageX);
            t.onKill = () =>
            {
                s_allTweenListX.Remove(node);
                tweenList.Remove(node);
            };
            return t;
        }

        public static void StopAllTween(this GObject target)
        {
            if (s_allTweenA.ContainsKey(target))
            {
                var tweenList = s_allTweenA[target];
                for (var i = tweenList.Count - 1; i >= 0; i--)
                {
                    tweenList[i].onKill = null;
                    tweenList[i].Kill();
                }
                tweenList.Clear();
                s_allTweenA.Remove(target);
                target.onDisposed.Remove(OnTargetRemovedFromStage);
            }

            if (s_allTweenX.ContainsKey(target))
            {
                var tweenList = s_allTweenX[target];
                for (var i = tweenList.Count - 1; i >= 0; i--)
                {
                    s_allTweenListX.Remove(tweenList[i].Value);
                    tweenList[i].Value.onKill = null;
                    tweenList[i].Value.Kill();
                }
                tweenList.Clear();
                s_allTweenX.Remove(target);
                target.onDisposed.Remove(OnTargetRemovedFromStageX);
            }
        }

        public static Tween RunTween(this Transform target, TWAction action)
        {
            var hashCode = target.GetHashCode();
            if (!s_allTweenB.ContainsKey(hashCode))
            {
                s_allTweenB.Add(hashCode, new List<Tween>());
            }

            var tweenList = s_allTweenB[hashCode];
            var t = action.ApplyTween(target);
            tweenList.Add(t);
            t.onKill = () =>
            {
                tweenList.Remove(t);
                if (tweenList.Count == 0)
                {
                    s_allTweenB.Remove(hashCode);
                }
            };
            return t;
        }
        
        public static Tween RunTweenX(this Transform target, TWAction action)
        {
            var hashCode = target.GetHashCode();
            if (!s_allTweenY.ContainsKey(hashCode))
            {
                s_allTweenY.Add(hashCode, new List<LinkedListNode<Tween>>());
            }
            
            var tweenList = s_allTweenY[hashCode];
            var t = action.ApplyTween(target);
            t.SetUpdate(UpdateType.Manual);
            var node = s_allTweenListX.AddLast(t);
            tweenList.Add(node);
            t.onKill = () =>
            {
                s_allTweenListX.Remove(node);
                tweenList.Remove(node);
            };
            return t;
        }
        
        public static Tween RunTweenX(this Rigidbody target, TWAction action)
        {
            var hashCode = target.GetHashCode();
            if (!s_allTweenY.ContainsKey(hashCode))
            {
                s_allTweenY.Add(hashCode, new List<LinkedListNode<Tween>>());
            }
            
            var tweenList = s_allTweenY[hashCode];
            var t = action.ApplyTween(target);
            t.SetUpdate(UpdateType.Manual);
            var node = s_allTweenListX.AddLast(t);
            tweenList.Add(node);
            t.onKill = () =>
            {
                s_allTweenListX.Remove(node);
                tweenList.Remove(node);
            };
            return t;
        }

        public static void StopAllTween(this Transform target)
        {
            var hashCode = target.GetHashCode();
            if (s_allTweenB.ContainsKey(hashCode))
            {
                var tweenList = s_allTweenB[hashCode];
                for (var i = tweenList.Count - 1; i >= 0; i--)
                {
                    tweenList[i].onKill = null;
                    tweenList[i].Kill();
                }

                tweenList.Clear();
                s_allTweenB.Remove(hashCode);
            }
            
            if (s_allTweenY.ContainsKey(hashCode))
            {
                var tweenList = s_allTweenY[hashCode];
                for (var i = tweenList.Count - 1; i >= 0; i--)
                {
                    s_allTweenListX.Remove(tweenList[i].Value);
                    tweenList[i].Value.onKill = null;
                    tweenList[i].Value.Kill();
                }
                tweenList.Clear();
                s_allTweenY.Remove(hashCode);
            }
        }

        public static void UpdateTweenX(float dt)
        {
            var node = s_allTweenListX.First;
            while (node != null)
            {
                var tween = node.Value;
                if (tween.IsActive())
                {
                    tween.Goto(tween.position+dt);
                    if (!tween.IsActive() || tween.IsComplete())
                    {
                        node = node.Next;
                        tween.Kill();
                    }
                    else
                    {
                        node = node.Next;
                    }
                }
            }
        }
    }
}