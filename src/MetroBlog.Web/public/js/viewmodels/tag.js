define(function (require, exports, module) {
    var articleService = require("../http/articleService");
    var $ = require("../modules/jquery");
    require("../modules/jquery.actual");
    var ko = require("../modules/knockout");
    var layer = require("../modules/layer/layer");
    var laypage = require("../modules/laypage/laypage");
    var template = require("../modules/template");
    var common = require("common");
    var viewModel = {
        tagList: ko.observableArray([]),
        pageIndex: ko.observable(1),
        pageSize: ko.observable(40),
        events: {}
    };


    function changeType(json) {
        var tagItem = {};
        for (var key in json) {
            tagItem[key] = ko.observable(json[key]);
        }
        return tagItem;
    }

    viewModel.events.saveData = function ($data, event, errorcall) {
        var data = $.parseJSON(ko.toJSON($data));
        var deferred = articleService.editTag(data);
        $.when(deferred).done(function (response) {
            if (response.error) {
                if (typeof (errorcall) == 'function') {
                    errorcall();
                }
                common.msg(response.message);
                return;
            }
            common.msg("数据已更新");
        }).fail(function (error) {
            console.log(error);
        });

    }
    viewModel.events.refresh = function () {
        var deferred = articleService.tagList(viewModel.pageIndex(), viewModel.pageSize());
        $.when(deferred).done(function (response) {
            if (response.error) {
                common.msg(response.message);
                return;
            }
            initPage(response.pageCount);
            viewModel.tagList.removeAll();
            $(response.data).each(function (index) {
                viewModel.tagList.push(changeType(response.data[index]));
            });
        }).fail(function (error) {
            console.log(error);
        });
    }
    viewModel.events.remove = function ($data, event) {
        common.confirm("确认移除标签吗？", ["确认", "取消"], function () {
            $data.delete = true;
            viewModel.events.saveData($data, event);
        });
    }
    viewModel.events.edit = function ($data, event) {
        layer.prompt({ title: '编辑标签', formType: 0, value: $data.tagName(), maxlength: 7 }, function (text) {
            if (text == '' || text.length == 0) {
                common.msg("输入不能为空！");
                return;
            }
            var temp = $data.tagName();
            $data.tagName(text);
            viewModel.events.saveData($data, event, function () {
                $data.tagName(temp);
            });
        });
    }
    function initPage(pageCount) {
        if (pageCount == 1) {
            return;
        }
        var page = $("#page");
        if (!page.attr("init")) {
            page.attr("init", true);
            window.page = true
            laypage({
                cont: $("#page"), //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                pages: pageCount, //通过后台拿到的总页数
                curr: viewModel.pageIndex() || 1, //当前页
                jump: function (obj, first) { //触发分页后的回调
                    if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                        viewModel.pageIndex(obj.curr);
                        viewModel.events.refresh();
                    }
                }
            });
        }
    }
    viewModel.events.refresh();
    ko.applyBindings(viewModel);
});