define(function (require, exports, module) {
    var $ = require("../modules/jquery");
    return {
        list: function () {
            var url = "/theme/list/";
            return $.ajax({
                url: url,
                type: "GET",
                dataType: "json"
            });
        },
        fileList: function (data) {
            var url = "/theme/filelist/";
            return $.ajax({
                url: url,
                type: "GET",
                data: data,
                dataType: "json"
            });
        },
        getFile: function (data) {
            var url = "/theme/getfile/";
            return $.ajax({
                url: url,
                type: "GET",
                data: data,
                dataType: "json"
            });
        },
        saveFile: function (data) {
            var url = "/theme/savefile/";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        },
        compile: function (data) {
            var url = "/theme/compile/";
            return $.ajax({
                url: url,
                type: "GET",
                data: data,
                dataType: "json"
            });
        }


    };
});