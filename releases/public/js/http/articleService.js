define(function (require, exports, module) {
    var $ = require("../modules/jquery");
    return {
        list: function (data) {
            var url = "/api/articlelist/";
            return $.ajax({
                url: url,
                type: "GET",
                data: data,
                dataType: "json"
            });
        },
        save: function (data) {
            var url = "/api/savearticle/";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        },
        categoryList: function () {
            var url = "/api/categorylist/";
            return $.ajax({
                url: url,
                type: "GET",
                dataType: "json"
            });
        },
        saveCategory: function (data) {
            var url = "/api/savecategory/";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        },
        updateCategoryCount: function (data) {
            var url = "/api/updatecategorycount/";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        },
        tagList: function (pageIndex, pageSize) {
            var url = "/api/tag/";
            return $.ajax({
                url: url,
                type: "GET",
                data: { pageIndex: pageIndex, pageSize: pageSize },
                dataType: "json"
            });
        },
        editTag: function (data) {
            var url = "/api/edittag/";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        },
        removeTag: function (data) {
            var url = "/api/removetag/";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        }
    };
});