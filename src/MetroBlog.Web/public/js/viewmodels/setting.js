define(function (require, exports, module) {
    var settingService = require("../http/settingService");
    var $ = require("../modules/jquery");
    var ko = require("../modules/knockout");
    var common = require("common");
    var viewModel = {
        events: {}
    };
    viewModel.events.submitForm = function (d, e) {
        alert(0);
        var data = $.parseJSON(ko.toJSON(d));
        var deferred = settingService.save(data);
        $.when(deferred).done(function (response) {

            if (response.error) {
                common.msg(response.message);
                return;
            }
            common.msg("ok");
        }).fail(function (error) {
            console.log(error);
        });
    }
    var deferred = settingService.list({});
    $.when(deferred).done(function (response) {
        for (var k in response) {
            var value = response[k];
            viewModel[k] = ko.observable(value);
        }

        ko.applyBindings(viewModel);
    }).fail(function (error) {
        console.log(error);
    });



});