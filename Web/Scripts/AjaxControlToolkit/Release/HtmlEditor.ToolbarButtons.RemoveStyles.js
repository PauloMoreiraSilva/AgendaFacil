Type.registerNamespace("Sys.Extended.UI.HtmlEditor.ToolbarButtons"),Sys.Extended.UI.HtmlEditor.ToolbarButtons.RemoveStyles=function(e){Sys.Extended.UI.HtmlEditor.ToolbarButtons.RemoveStyles.initializeBase(this,[e])},Sys.Extended.UI.HtmlEditor.ToolbarButtons.RemoveStyles.prototype={callMethod:function(){if(!Sys.Extended.UI.HtmlEditor.ToolbarButtons.RemoveStyles.callBaseMethod(this,"callMethod"))return!1;var e=this._designPanel;setTimeout(function(){function t(e,l,n,d){var r=n.cloneNode(!1);if(e)if(e.push&&"function"==typeof e.push)for(var i=0;i<e.length;i++)r.appendChild(e[i]);else r.appendChild(e);for(;l;){var o=d?l.previousSibling:l.nextSibling;(1==l.nodeType||3==l.nodeType&&Sys.Extended.UI.HtmlEditor.Trim(""+l.data).length>0)&&(1==l.nodeType&&(Sys.Extended.UI.HtmlEditor.isStyleTag(l.tagName)&&"A"!=l.tagName.toUpperCase()&&(!l.id||"_left_"!=l.id&&"_right_"!=l.id)&&Sys.Extended.UI.HtmlEditor.spanJoiner(l),Sys.Extended.UI.HtmlEditor.isStyleTag(l.tagName)&&0==l.childNodes.length&&(!l.id||"_left_"!=l.id&&"_right_"!=l.id)&&(l=null)),l&&(0!=r.childNodes.length&&d?r.insertBefore(l,r.firstChild):r.appendChild(l))),l=o}if(0==r.childNodes.length)delete r,r=null;else if(1==r.childNodes.length&&3==r.firstChild.nodeType&&0==(""+r.firstChild.data).length)delete r,r=null;else if(Sys.Extended.UI.HtmlEditor.isStyleTag(r.tagName)){for(var a=r.childNodes.length,s=0;s<r.childNodes.length;s++){var _=r.childNodes.item(s);1!=_.nodeType||Sys.Extended.UI.HtmlEditor.isStyleTag(_.tagName)||a--}if(0==a){for(var f=[];r.firstChild;){var _=r.removeChild(r.firstChild);f.push(_)}r=f}}return n==B?r:t(r,d?n.previousSibling:n.nextSibling,n.parentNode,d)}function l(e,t){for(;e;){if(e==f)return void(s=!0);if(3==e.nodeType){for(;e.nextSibling&&3==e.nextSibling.nodeType;)e.data=""+e.data+e.nextSibling.data,e.parentNode.removeChild(e.nextSibling);Sys.Extended.UI.HtmlEditor.Trim(""+e.data).length>0&&i.push(e)}else l(e.firstChild,!1);if(s)return;e.parentNode;if(t)for(;e&&null==e.nextSibling;)e=e.parentNode;e=e.nextSibling}}var n=Sys.Extended.UI.HtmlEditor.isIE?"":Sys.Extended.UI.HtmlEditor.Trim(e.getSelectedHTML()),d=e._getSelection(),r=e._createRange(d),i=null,o=!1;if(!e.isControl()&&(Sys.Extended.UI.HtmlEditor.isIE&&r.text.length>0||!Sys.Extended.UI.HtmlEditor.isIE&&n.length>0)?i=e._getTextNodeCollection():(i=e._tryExpand(),o=!0),null!=i&&i.length>0){var a=!1,s=!0;e._saveContent();var _=e._doc.createElement("span");_.id="_left_";var f=e._doc.createElement("span");f.id="_right_";var h=i[0].parentNode,g=i[i.length-1].parentNode;for(h.insertBefore(_,i[0]),i[i.length-1].nextSibling?g.insertBefore(f,i[i.length-1].nextSibling):g.appendChild(f);s;){s=!1;for(var m=0;m<i.length;m++){var p=i[m].parentNode;if(p&&null==i[m].previousSibling&&null==i[m].nextSibling){var E=p.tagName.toUpperCase();if(Sys.Extended.UI.HtmlEditor.isStyleTag(E)&&"A"!=E&&(p.className!=Sys.Extended.UI.HtmlEditor.smartClassName||"H"==E.substr(0,1))){var v=Sys.Extended.UI.HtmlEditor.differAttr(p,["class","color","face","size"]);if(a=!0,0==v.length){var c=p.parentNode,S=p.firstChild?p.firstChild:null,y=null;if("H"==E.toUpperCase().substr(0,1)&&Sys.Extended.UI.HtmlEditor.isIE)for(y=e._doc.createElement("p"),y.className=Sys.Extended.UI.HtmlEditor.smartClassName,c.insertBefore(y,p);p.firstChild;)y.appendChild(p.firstChild);else{for(;p.firstChild;)c.insertBefore(p.firstChild,p);if("H"==E.toUpperCase().substr(0,1)){var u=e._doc.createElement("br");c.insertBefore(u,p)}}c.removeChild(p),s=!0}else{for(var c=p.parentNode,N=e._doc.createElement(E),x=0;x<v.length;x++)N.setAttribute(v[x][0],v[x][1]);for(c.insertBefore(N,p);p.firstChild;)N.appendChild(p.firstChild);c.removeChild(p)}}}}}for(var m=0;m<i.length;m++){var S=i[m],C=null!=i[m].parentNode&&"undefined"!=typeof i[m].parentNode?i[m].parentNode:null;if(C){for(var B=null;C&&C.tagName&&"BODY"!=C.tagName.toUpperCase()&&Sys.Extended.UI.HtmlEditor.isStyleTag(C.tagName)&&"A"!=C.tagName.toUpperCase()&&0==Sys.Extended.UI.HtmlEditor.differAttr(C,["class","color","face","size"]).length;)B=C,C=C.parentNode;if(B){a=!0,C=S.parentNode,null==S.previousSibling&&null==S.nextSibling&&C&&C.tagName&&"BODY"!=C.tagName.toUpperCase()&&Sys.Extended.UI.HtmlEditor.isStyleTag(C.tagName)&&Sys.Extended.UI.HtmlEditor.differAttr(C,["class","color","face","size"]).length>0&&(S=C);var I=t(null,S.previousSibling,S.parentNode,!0),U=t(null,S.nextSibling,S.parentNode,!1),p=B.parentNode;if(I){if(I.push&&"function"==typeof I.push)for(var T=0;T<I.length;T++)p.insertBefore(I[T],B);else p.insertBefore(I,B);Sys.Extended.UI.HtmlEditor.isIE&&(_=e._doc.getElementById("_left_"),f=e._doc.getElementById("_right_"))}if(p.insertBefore(S,B),U){if(U.push&&"function"==typeof U.push)for(var T=0;T<U.length;T++)p.insertBefore(U[T],B);else p.insertBefore(U,B);Sys.Extended.UI.HtmlEditor.isIE&&(_=e._doc.getElementById("_left_"),f=e._doc.getElementById("_right_"))}p.removeChild(B)}}}if(o){if(Sys.Extended.UI.HtmlEditor.isIE&&null!=e.__saveBM__){try{var H=_.parentNode;for(H.removeChild(_);H&&0==H.childNodes.length;)H.parentNode.removeChild(H),H=H.parentNode;for(H=f.parentNode,H.removeChild(f);H&&0==H.childNodes.length;)H.parentNode.removeChild(H),H=H.parentNode;_=null,f=null}catch(e){}var d=e._getSelection(),r=e._createRange(d);r.moveToBookmark(e.__saveBM__),r.select(),e.__saveBM__=null}else if(null!=e.__saveBM__){if(3==e.__saveBM__[0].nodeType){var d=e._getSelection(),r=e._doc.createRange();r.setStart(e.__saveBM__[0],e.__saveBM__[1]),r.setEnd(e.__saveBM__[0],e.__saveBM__[1]),e._removeAllRanges(d),e._selectRange(d,r)}else e._trySelect(e.__saveBM__[0],e.__saveBM__[0]),e.__saveBM__[0].parentNode.removeChild(e.__saveBM__[0]);e.__saveBM__=null}}else if(Sys.Extended.UI.HtmlEditor.isIE)try{d=e._getSelection();var b=e._createRange(d),M=e._createRange(d);b.moveToElementText(_),M.moveToElementText(f),b.setEndPoint("EndToEnd",M),b.select()}catch(e){}else{i=[];var s=!1;l(_,!0),r=e._doc.createRange(),r.setStart(i[0],0),r.setEnd(i[i.length-1],(""+i[i.length-1].data).length),e._removeAllRanges(d),e._selectRange(d,r)}try{var H;if(null!=_)for(H=_.parentNode,H.removeChild(_);H&&0==H.childNodes.length;)H.parentNode.removeChild(H),H=H.parentNode;if(null!=f)for(H=f.parentNode,H.removeChild(f);H&&0==H.childNodes.length;)H.parentNode.removeChild(H),H=H.parentNode}catch(e){}e.onContentChanged(),e._editPanel.updateToolbar()}},0)}},Sys.Extended.UI.HtmlEditor.ToolbarButtons.RemoveStyles.registerClass("Sys.Extended.UI.HtmlEditor.ToolbarButtons.RemoveStyles",Sys.Extended.UI.HtmlEditor.ToolbarButtons.MethodButton);