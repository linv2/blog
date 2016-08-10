define(function (require, exports, module) {
    var themeService = require("../http/themeService");
    //var $ = require("../modules/jquery");
    var ko = require("../modules/knockout");
    var template = require("../modules/template");
    var common = require("common");
    var url = require("../modules/url.helper");
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
                    var data = { "themeName": themeName, "fileName": treeNode.id };
                    var deferred = themeService.getFile(data);
                    $.when(deferred).done(function (response) {
                        tabs.add(treeNode.id)
                    }).fail(function (error) {

                    });
                }
            }
        }
    };
    var tabs = new Tabs("#tabs");

    function filter(treeId, parentNode, childNodes) {
        if (!childNodes) return null;
        for (var i = 0, l = childNodes.length; i < l; i++) {
            childNodes[i].name = childNodes[i].name.replace(/\.n/g, '.');
        }
        return childNodes;
    }

    window.zTree.init($("#treeDemo"), setting);
    //viewModel.events.edit = function (d, e) {
    //    window.location = 'themeitem?themeName=' + d;
    // }
    //viewModel.events.readDirectory("/");
    ko.applyBindings(viewModel);
});

var Tabs = function (container) {
    var container = $(container);
    var self = this;
    var TabItem = function (title) {
        var tabItemSelf = this;
        tabItemSelf.title = $("<li><a href='javascript:;'>" + title + "</a></li>");
        tabItemSelf.title.click(function () {
            container.find("ul.nav li.active").removeClass("active");
            $(this).addClass("active");
            container.find(".tile-body").hide();
            tabItemSelf.content.show();
        });
        tabItemSelf.content = $("<div class='tile-body rounded-bottom-corners'><textarea></textarea></div>").hide();
        tabItemSelf.editor = this.content.find("textarea");
        tabItemSelf.title.trigger("click");
        return this;
        //title.attr("title", title);
    };
    self.add = function (title) {
        var tabItem = new TabItem(title);
        container.find("ul.nav").append(tabItem.title);
        container.append(tabItem.content);
    }
}
