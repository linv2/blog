define(function (require, exports, module) {
    var $ = require("../modules/jquery");
    return {
        login: function (data) {
            var url = "/auth/";
            return $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "json"
            });
        }
    };
});