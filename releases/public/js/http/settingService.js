define(function (require, exports, module) {
    var $ = require("../modules/jquery");
    return {
        list: function (data) {
            var url = "/api/setting";
            return $.ajax({
                url: url,
                type: "GET",
                data: data,
                dataType: "json"
            });
        },
        save: function (data) {
            var url = "/api/setting";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        }
    };
});