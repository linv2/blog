define(function (require, exports, module) {
    var themeService = require("../http/themeService");
    //var $ = require("../modules/layer");
    var ko = require("../modules/knockout");
    var template = require("../modules/template");
    var common = require("common");
    var url = require("../modules/url.helper");
    require("../modules/context/jquery.context");
    var viewModel = {
        themes: ko.observableArray(),
        events: {}
    };
    var themeName = url.get("themename");



    var setting = {
        async: {
            enable: true,
            url: "/theme/filelist",
            autoParam: ["id"],
            otherParam: { "themeName": themeName },
            dataFilter: filter
        },
        callback: {
            beforeClick: function (treeId, treeNode) {
                if (!treeNode.isParent) {
                    var index = common.layer.open({
                        type: 2,
                        title: false,
                        maxmin: true,
                        shade: 0.8,
                        closeBtn: 1,
                        shadeClose: true,
                        content: "editTheme?skey=" + escape(treeNode.skey),
                        success: function (layero, index) {
                            $(layero).find("iframe").get(0).contentWindow.layerIndex = index;
                            $(layero).find("iframe").get(0).contentWindow.closeLayer = function () {
                                layer.close(this.layerIndex)
                            }
                            //alert();
                        }
                    });
                    layer.full(index);
                }
            }
        }
    };

    function filter(treeId, parentNode, childNodes) {
        if (!childNodes) return null;
        for (var i = 0, l = childNodes.length; i < l; i++) {
            childNodes[i].name = childNodes[i].name.replace(/\.n/g, '.');
        }
        return childNodes;
    }

    window.zTree.init($("#treeDemo"), setting);
    ko.applyBindings(viewModel);

    context.init({ preventDoubleContext: false });

    context.attach('#treeDemo', [
		{ text: '添加文件夹', href: 'javascript:;', action: function (o) { alert(0); } },
		{ text: '添加文件', href: 'javascript:;', action: function (o) { alert(0); } }
    ]);
});

