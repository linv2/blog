define(function (require, exports, module) {
    var $ = require("../modules/jquery");
    return {
        save: function (data) {
            var url = "/api/savemenu";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        },
        list: function (data) {
            var url = "/api/menu";
            return $.ajax({
                url: url,
                type: "GET",
                data: data,
                dataType: "json"
            });
        }
    };
});