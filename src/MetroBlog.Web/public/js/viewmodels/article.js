define(function (require, exports, module) {
    var articleService = require("../http/articleService");
    var $ = require("../modules/jquery");
    var ko = require("../modules/knockout");
    var layer = require("../modules/layer/layer");
    var template = require("../modules/template");
    require("../modules/jquery.tagsinput")($);
    var viewModel = {
        title: ko.observable(""),
        content: ko.observable(""),
        comment: ko.observable(true),
        keyWord: ko.observable(""),
        description: ko.observable(""),
        categoryId: ko.observable(0),
        tags: ko.observable(""),
        categorySource: ko.observableArray(),
        events: {},

    }


    var categorydeferred = articleService.categoryList();
    $.when(categorydeferred).done(function (response) {
        viewModel.categorySource(response);
    }).fail(function (error) {
        console.log(error);
    });

    $("#tags").tagsInput({ width: 'auto', defaultText: "添加标签" });
    ko.applyBindings(viewModel);


    viewModel.events.submitForm = function () {
        var data = $("#addForm").serialize();        
        var deferred = articleService.save(data);
        $.when(deferred).done(function (response) {
            
            if (response.error) {
                alert(response.message);
                return;
            }
        }).fail(function (error) {
            console.log(error);
        });
    };




});