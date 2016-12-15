define(function (require, exports, module) {
    var articleService = require("../http/articleService");
    var menuService = require("../http/menuService");
    var $ = require("../modules/jquery");
    require("../modules/jquery.actual");
    var ko = require("../modules/knockout");
    var layer = require("../modules/layer/layer");
    var template = require("../modules/template");
    var common = require("common");
    var viewModel = {
        category: ko.observableArray(),
        menu: ko.observableArray(),
        targetType: function (target) {
            for (var i = 0; i < targetType.length; i++) {
                if (target == targetType[i].val) {
                    return targetType[i].text;
                }
            }
            return target;
        },
        events: {}
    };

    var targetType = [
        { 'text': '当前页面打开', 'val': '_self' },
        { 'text': '新页面打开', 'val': '_blank' },
        { 'text': '自定义', 'val': 'custom' },
    ];


    var h = $("#editDialog").actual('height');

    var categoryList = articleService.categoryList();
    $.when(categoryList).done(function (response) {
        $(response).each(function (index) {
            viewModel.category.push(response[index]);
        });
    }).fail(function (error) {
        console.log(error);
    });

    viewModel.events.add = function () {
        var dialog = $("#editDialog").clone().html();
        var addModel = {
            id: ko.observable(0),
            name: ko.observable(),
            sortId: ko.observable(0),
            link: ko.observable(),
            target: ko.observable("_self"),
            targetValue: ko.observable('_self'),
            targetDs: ko.observableArray(targetType),

            targetTypeChange: function (d, e) {
                if (e.target.value == 'custom') {
                    d.target('');
                } else {
                    d.target(e.target.value);
                }
                return d.targetShow(d.target() == '');
            },
            targetShow: ko.observable(false),
            category: viewModel.category(),
            categoryId: ko.observable(0),
            linkTypeChange: function (d, e) {
                return d.showLink(d.categoryId() == undefined);
            },
            showLink: ko.observable(true),
            closeDialog: function (d, e) {
                layer.close(d.index);
            },
            saveData: viewModel.events.saveData,
        }
        layer.open({
            type: 1,
            area: ["500px", h], //宽高
            title: false,
            closeBtn: 0,
            shade: 0.1,
            shadeClose: false,
            content: dialog,
            success: function (layero, index) {
                if (layero.push) {
                    layero = layero[0];
                }
                addModel.dialog = layero;
                addModel.index = index;
                ko.applyBindings(addModel, layero);
            }
        });
    }
    viewModel.events.saveData = function ($data, event) {
        var data = $($data.dialog).find("form").serialize();
        var deferred = menuService.save(data);
        $.when(deferred).done(function (response) {
            if (response.error) {
                common.msg(response.message);
                return;
            }
            common.msg("数据已保存");
            viewModel.events.refresh();
            $data.closeDialog($data);
        }).fail(function (error) {
            console.log(error);
        });
    }
    viewModel.events.refresh = function () {
        var deferred = menuService.list();
        $.when(deferred).done(function (response) {
            viewModel.menu(response);
        }).fail(function (error) {
            console.log(error);
        });
    }
    viewModel.events.edit = function ($data, event) {
        var dialog = $("#editDialog").clone().html();
        if (typeof ($data.target) != "function") {
            $data.targetValue = ko.observable();
            $data.target = ko.observable($data.target);
            $data.targetDs = ko.observableArray(targetType);
            $data.targetTypeChange = function (d, e) {
                if (e.target.value === "custom") {
                    d.target("");
                } else {
                    d.target(e.target.value);
                }
                return d.targetShow(d.target() == "");
            };

            $data.category = viewModel.category(),
            $data.linkTypeChange = function (d, e) {
                $data.categoryId = e.target.value;
              //  alert(d.categoryId);
                return d.showLink(!d.categoryId);
            };
            $data.closeDialog = function (d, e) {
                layer.close(d.index);
            }
            $data.saveData = viewModel.events.saveData;
        }
        $data.targetShow = ko.observable(!($data.target() == '_self' || $data.target() == '_blank'));
        $data.showLink = ko.observable($data.categoryId == undefined || $data.categoryId == 0);
        $data.targetValue(((($data.target() == '_self' || $data.target() == '_blank')) ? $data.target() : "custom"));
        layer.open({
            type: 1,
            area: ["500px", h], //宽高
            title: false,
            closeBtn: 0,
            shade: 0.1,
            shadeClose: false,
            content: dialog,
            success: function (layero, index) {
                if (layero.push) {
                    layero = layero[0];
                }
                $data.dialog = layero;
                $data.index = index;

                ko.applyBindings($data, layero);
            }
        });

    };
    viewModel.events.remove = function ($data, event) {
        common.confirm("确认移除分类吗？", ["确认", "取消"], function () {
            $data.hidden = true;
            var data = $.parseJSON(ko.toJSON($data));
            var deferred = menuService.save(data);
            $.when(deferred).done(function (response) {
                if (response.error) {
                    common.msg(response.message);
                    return;
                }
                common.msg("数据已保存");
                viewModel.events.refresh();
                $data.closeDialog($data);
            }).fail(function (error) {
                console.log(error);
            });
        });
    };


    viewModel.events.refresh();
    ko.applyBindings(viewModel, $("#menulist").get(0));
});