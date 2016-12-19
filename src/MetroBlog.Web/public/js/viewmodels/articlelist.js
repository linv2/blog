define(function (require, exports, module) {
    var articleService = require("../http/articleService");
    var $ = require("../modules/jquery");
    var ko = require("../modules/knockout");
    var laypage = require("../modules/laypage/laypage");
    var layer = require("../modules/layer/layer");
    var viewModel = {
        categoryId: ko.observable(0),
        status: ko.observable(-1),
        keyWord: ko.observable(""),
        _tagName: ko.observable(""),
        pageIndex: ko.observable(1),
        pageSize: ko.observable(1),
        categorySource: ko.observableArray(),
        articleList: ko.observableArray(),
        events: {}

    }
    viewModel.events.headerShow = ko.observable("");
    viewModel.events.footerShow = ko.observable("");

    function page(pageSize, curr) {
        $("#page").empty();
        laypage({
            cont: "page",
            pages: pageSize,
            curr: curr,
            jump: function (obj, first) {
                if (!first) {
                    viewModel.pageIndex(obj.curr);
                    viewModel.events.loadNext();
                }
            }
        });
    }
    viewModel.events.filter = function () {
        viewModel.pageIndex(1);
        viewModel.events.loadNext();
    };

    function getSearchParam() {
        var search = {
            keyWord: viewModel.keyWord(),
            categoryId: viewModel.categoryId(),
            pageIndex: viewModel.pageIndex(),
            pageSize: viewModel.pageSize()
        };
        return search;
    }
    viewModel.events.switchCategory = function (data, event) {
        var target = $(event.target).parents("li");
        if (!target.hasClass("active")) {
            $("li.active").removeClass("active");
            target.addClass("active");
            viewModel.categoryId(data.id ? data.id : 0);
            viewModel.pageIndex(1);
            viewModel.events.loadNext();
        }
    }
    viewModel.events.loadNext = function () {
        var data = getSearchParam();
        var deferred = articleService.list(data);
        $.when(deferred).done(function (response) {
            if (response.error) {
                return;
            }
            response = response.data;
            viewModel.events.headerShow("共有" + response.rows + "条数据，已加载" + (((response.pageIndex - 1) * response.pageSize) + response.data.length) + "条");
            viewModel.events.footerShow("共有" + response.rows + "条数据，计" + response.pageCount + "页");
            viewModel.articleList(response.data);
            page(response.pageCount, response.pageIndex);
        }).fail(function (error) {
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




});