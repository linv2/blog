
define(function (require, exports, module) {

    var articleService = require("../http/articleService");
    var editormd = require("../../editormd/editormd");
    var ko = require("../modules/knockout");
    var url = require("../modules/url.helper");
    var common = require("common");
    require("../modules/jquery.tagsinput")($);
    var viewModel = {
        id: ko.observable(0),
        title: ko.observable(""),
        alias: ko.observable(""),
        content: ko.observable(""),
        comment: ko.observable(true),
        keyWord: ko.observable(""),
        description: ko.observable(""),
        categoryId: ko.observable(0),
        tags: ko.observable(""),
        categorySource: ko.observableArray(),
        events: {}
    }


    var categorydeferred = articleService.categoryList();
    $.when(categorydeferred).done(function (response) {
        viewModel.categorySource(response);
        var id = url.get("id");
        if (id) {
            $.when(articleService.get(id)).done(function (response) {
                for (var k in viewModel) {
                    if (response[k])
                        viewModel[k](response[k]);
                }
                var tagArr = new Array();
                for (var index in response.tags) {
                    tagArr.push(response.tags[index].tagName);
                }
                viewModel.tags(tagArr.join(","));
                $("#tags").tagsInput({ width: 'auto', defaultText: "添加标签" });
            }).fail(function (error) {
                console.log(error);
            });
        } else {
            $("#tags").tagsInput({ width: 'auto', defaultText: "添加标签" });
        }

    }).fail(function (error) {
        console.log(error);
    });

    //  $("#tags").tagsInput({ width: 'auto', defaultText: "添加标签" });
    ko.applyBindings(viewModel);

    viewModel.events.submitForm = function () {
        var data = $("#addForm").serialize();
        var deferred = articleService.save(data);
        $.when(deferred).done(function (response) {
            if (response.error) {
                common.msg(response.message);
                common.redirect("articlelist");
            }
        }).fail(function (error) {
            console.log(error);
        });
    };

    viewModel.events.showEditor = function () {
        var index = common.layer.open({
            type: 2,
            title: false,
            maxmin: true,
            shade: 0.8,
            closeBtn: 1,
            shadeClose: true,
            content: "MarkDown",
            success: function (layero, index) {
                var contentWin = $(layero).find("iframe").get(0).contentWindow;
                contentWin.layerIndex = index;
                contentWin.closeLayer = function () {
                    common.layer.close(this.layerIndex);
                }
                contentWin.getContent = function () {
                    return viewModel.content();
                }
                contentWin.setContent = function (content) {
                    viewModel.content(content);
                }
                if (contentWin.load)
                    contentWin.load();

            }
        });
        common.layer.full(index);
    }

    viewModel.events.toList = function () {
        common.redirect("articlelist");
    }
    viewModel.events.extractDescription = function () {
        $("#markdown").empty();
        var editormdView = editormd.markdownToHTML("markdown", {
            markdown: viewModel.content()
        });
        viewModel.description($("#markdown").text());
    }
});