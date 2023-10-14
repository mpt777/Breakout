using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{

    public class Animation<T>
    {
        private bool _active = true;
        private T _obj;
        private String _method;
        private TimeSpan _span;
        private float _accumulator;
        public Animation(T obj, String method, TimeSpan span)
        {
            this._obj = obj;
            this._method = method;
            this._span = span;
        }
        private void Invoke()
        {
            Type type = this._obj.GetType();
            MethodInfo theMethod = type.GetMethod(this._method);
            theMethod.Invoke(_obj, null);
        }
        public void Start()
        {
            this._active = true;
        }
        public void Stop()
        {
            this._active = false;
        }
        public void Reset()
        {
            this._accumulator = 0f;
        }
        private void CheckIfInvoke()
        {
            if (this._accumulator >= this._span.TotalSeconds)
            {
                this._accumulator -= (float)this._span.TotalSeconds;
                this.Invoke();
            }
        }
        public void Update(GameTime gameTime)
        {
            if (this._active) 
            {
                this._accumulator += (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.CheckIfInvoke();
            }
            
        }
    }
}
