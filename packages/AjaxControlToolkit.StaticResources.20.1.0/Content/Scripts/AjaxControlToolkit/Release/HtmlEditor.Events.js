Type.registerNamespace("Sys.Extended.UI.HtmlEditor"),Sys.Extended.UI.HtmlEditor.ActiveModeChangedArgs=function(e,t,d){if(3!=arguments.length)throw Error.parameterCount();Sys.Extended.UI.HtmlEditor.ActiveModeChangedArgs.initializeBase(this),this._oldMode=e,this._newMode=t,this._editPanel=d},Sys.Extended.UI.HtmlEditor.ActiveModeChangedArgs.prototype={get_oldMode:function(){return this._oldMode},get_newMode:function(){return this._newMode},get_editPanel:function(){return this._editPanel}},Sys.Extended.UI.HtmlEditor.ActiveModeChangedArgs.registerClass("Sys.Extended.UI.HtmlEditor.ActiveModeChangedArgs",Sys.EventArgs);