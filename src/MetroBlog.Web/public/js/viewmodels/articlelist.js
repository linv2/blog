define(function (require, exports, module) {
    var articleService = require("../http/articleService");
    var $ = require("../modules/jquery");
    var ko = require("../modules/knockout");
    var layer = require("../modules/layer/layer");
    var template = require("../modules/template");
    var viewModel = {
        categoryId: ko.observable(0),
        status: ko.observable(-1),
        keyWord: ko.observable(""),
        _tagName: ko.observable(""),
        pageIndex: ko.observable(1),
        pageSize: ko.observable(10),
        categorySource: ko.observableArray(),
        events: {},

    }
    var lock = false;
    viewModel.events.headerShow = ko.observable("");
    viewModel.events.footerShow = ko.observable("");

    var articlescontent = $("#articlescontent");
    viewModel.events.filter = function () {
        lock = false;
        viewModel.pageIndex(1);
        articlescontent.empty();
        viewModel.events.loadNext();
    };
    viewModel.events.loadNext = function () {
        if (lock) {
            return;
        }
        lock = true;
        var data = $("#filterForm").serialize();
        var deferred = articleService.list(data);
        $.when(deferred).done(function (response) {
            lock = false;
            if (response.error) {
                return;
            }
            response = response.data;

            viewModel.events.headerShow("共有" + response.rows + "条数据，已加载" + (((response.pageIndex - 1) * response.pageSize) + response.data.length) + "条，点击加入筛选条件");

            viewModel.events.footerShow("共有" + response.rows + "条数据，计" + response.pageCount + "页，点击加载下一页");
            var render = template("articletemplete", { list: response.data, catetory: window.catetory });
            articlescontent.append($(render));
            if (response.pageIndex >= response.pageCount) {
                lock = true;
                viewModel.events.footerShow("数据已全部加载完成");
                return;
            }
        }).fail(function (error) {
            lock = false;
            console.log(error);
        });
    }

    var categorydeferred = articleService.categoryList();

    $.when(categorydeferred).done(function (response) {
        viewModel.categorySource(response);
    }).fail(function (error) {
        console.log(error);
    });


    ko.applyBindings(viewModel);
    viewModel.events.loadNext();




    template.helper('formatDate', function (input) {
        if (/^\d{4}-\d{2}-\d{2}T/.exec(input)) {
            return input.substr(0, 10) + " " + input.substr(11, 5);
        } else {
            return input;
        }
    });


});