Sys.ComponentSet=function(e,t,n){this._elementSet=e||(e=new Sys.ElementSet),this._components=this._execute(e,t,n)},Sys.ComponentSet.prototype={__class:!0,setProperties:function(e){return this.each(function(){Sys._set(this,e)})},get:function(e){var t=this._components;return"undefined"==typeof e?Array.apply(null,t):t[e||0]||null},each:function(e){return foreach(this._components,function(t,n){if(e.call(t,n)===!1)return!0}),this},elements:function(){return this._elementSet},_execute:function(e,t,n){function o(e){var n;return e instanceof t||(n=e.constructor)&&(n===t||n.inheritsFrom&&n.inheritsFrom(t)||n.implementsInterface&&n.implementsInterface(t))}var r=[];return t instanceof Array?r.push.apply(r,t):e.each(function(){var e=this.control;!e||t&&!o(e)||r.push(e),foreach(this._behaviors,function(e){t&&!o(e)||r.push(e)})}),"undefined"!=typeof n&&(r=r[n]?[r[n]]:[]),r}};