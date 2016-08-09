define(function (require, exports, module) {
    var articleService = require("../http/articleService");
    var $ = require("../modules/jquery");
    require("../modules/jquery.actual");
    var ko = require("../modules/knockout");
    var layer = require("../modules/layer/layer");
    var template = require("../modules/template");
    var common = require("common");
    var viewModel = {
        category: ko.observableArray(),
        events: {}
    };

    var h = $("#editDialog").actual('height');

    viewModel.events.add = function () {


        var addModel = {
            name: ko.observable(),
            keyWord: ko.observable(),
            description: ko.observable(),
            sortId: ko.observable(0),
            closeDialog: function (d, e) {
                layer.close(d.index);
            },
            saveData: viewModel.events.saveData
        }
        var dialog = $("#editDialog").clone().html();
        layer.open({
            type: 1,
            area: ["500px", h], //宽高
            title: false,
            closeBtn: 0,
            skin: 'layui-layer-nobg', //没有背景色
            shade: 0.1,
            shadeClose: false,
            content: dialog,
            success: function (layero, index) {
                if (layero.push) {
                    layero = layero[0];
                }
                addModel.index = index;
                ko.applyBindings(addModel, layero);
            }
        });
    }
    viewModel.events.saveData = function ($data, event) {
        var data = $.parseJSON(ko.toJSON($data));
        var deferred = articleService.saveCategory(data);
        $.when(deferred).done(function (response) {
            if (response.error) {
                common.msg(response.message);
                return;
            }
            common.msg("数据已保存");
            viewModel.events.refresh();
            d.closeDialog(d);
        }).fail(function (error) {
            console.log(error);
        });
    }
    viewModel.events.refresh = function () {
        var deferred = articleService.categoryList();
        $.when(deferred).done(function (response) {
            viewModel.category(response);
        }).fail(function (error) {
            console.log(error);
        });
    }
    viewModel.events.edit = function ($data, event) {
        if (!$data.edit) {
            $data.saveData = viewModel.events.saveData;
            $data.index = 0;
            $data.closeDialog = function (d, e) {
                layer.close(d.index);
            }
        }
        var dialog = $("#editDialog").clone().html();
        layer.open({
            type: 1,
            area: ["500px", h], //宽高
            title: false,
            closeBtn: 0,
            skin: 'layui-layer-nobg', //没有背景色
            shade: 0.1,
            shadeClose: false,
            content: dialog,
            success: function (layero, index) {
                if (layero.push) {
                    layero = layero[0];
                }
                $data.index = index;
                ko.applyBindings($data, layero);
            }
        });

    };
    viewModel.events.updateCount = function ($data) {
        var data = $.parseJSON(ko.toJSON($data));
        var deferred = articleService.updateCategoryCount(data);
        $.when(deferred).done(function (response) {
            if (response.error) {
                common.msg(response.message);
                return;
            }
            common.msg("数据已更新");
            viewModel.events.refresh();
        }).fail(function (error) {
            console.log(error);
        });
    };
    viewModel.events.remove = function ($data, event) {
        common.confirm("确认移除分类吗？", ["确认", "取消"], function () {
            $data.delete = true;
            viewModel.events.saveData($data, event);
        });
    };


    viewModel.events.refresh();
    ko.applyBindings(viewModel, $("#categorylist").get(0));
});