define(function (require, exports, module) {
    var userService = require("../http/userService");
    var $ = require("../modules/jquery");
    var ko = require("../modules/knockout");
    var common = require("common");
    var viewModel = {
        UserName: ko.observable(""),
        PassWord: ko.observable(""),
        events: {},
    }
    viewModel.events.submitForm = function () {
        var data = $.parseJSON(ko.toJSON(viewModel));
        var deferred = userService.login(data);
        $.when(deferred).done(function (response) {
            if (response.error) {
                common.msg(response.message, { time: 1500 });
            } else {
                window.location = "Main";
            }
        }).fail(function (error) {
            console.log(error);
        });
        return false;
    };
    ko.applyBindings(viewModel);
});