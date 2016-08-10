define(function (require, exports, module) {
    var themeService = require("../http/themeService");
    var $ = require("../modules/jquery");
    var ko = require("../modules/knockout");
    var template = require("../modules/template");
    var common = require("common");
    var viewModel = {
        themes: ko.observableArray(),
        events: {}
    };


    viewModel.events.refresh = function () {
        var deferred = themeService.list();
        $.when(deferred).done(function (response) {
            if (response.error) {
                common.msg(response.message);
                return;
            }
            $(response.data).each(function (index) {
                viewModel.themes.push(response.data[index]);
            });
        }).fail(function (error) {
            console.log(error);
        });
    }

    viewModel.events.edit = function (d, e) {
        window.location = 'themeitem?themeName=' + d;
    }
    viewModel.events.refresh();
    ko.applyBindings(viewModel);
});