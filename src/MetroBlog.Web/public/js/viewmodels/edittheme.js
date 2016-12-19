define(function (require, exports, module) {
    var themeService = require("../http/themeService");
    var ko = require("../modules/knockout");
    var template = require("../modules/template");
    var common = require("common");
    var url = require("../modules/url.helper");

    var modelist = require("../../ace/lib/ext/modelist");


    var viewModel = {
        skey: ko.observable(),
        file: ko.observable(),
        content: ko.observable(),
        events: {}
    };

    var skey = unescape(url.get("skey"));
    viewModel.skey(skey);
    var deferred = themeService.getFile({ "skey": skey });
    $.when(deferred).done(function (response) {
        if (response.error) {
            common.msg(response.message);
            return;
        }
        viewModel.content(response.data.content);


        document.title = response.data.file;
        viewModel.file(response.data.file);
        var editor = ace.edit("editor");
        editor.setTheme("ace/theme/solarized_dark");
        var mode = modelist.getModeForPath(viewModel.file()).mode;
        editor.session.setMode(mode);
        editor.commands.addCommand({
            name: "showKeyboardShortcuts",
            bindKey: { win: "Ctrl-Alt-h", mac: "Command-Alt-h" },
            exec: function (editor) {
                ace.config.loadModule("ace/ext/keybinding_menu", function (module) {
                    module.init(editor);
                    editor.showKeyboardShortcuts()
                })
            }
        });
        editor.commands.addCommand({
            name: "saveCode",
            bindKey: { win: "Ctrl-s", mac: "Command-s" },
            exec: function (editor) {
                var saveDeferred = themeService.saveFile({ "skey": skey, "content": editor.getValue() });
                $.when(saveDeferred).done(function (response) {
                    if (response.error) {
                        common.msg(response.message, { time: 2000 });
                        return;
                    } else {
                        common.msg("保存成功");
                    }

                }).fail(function (error) {
                    console.log(error);
                });
            }
        });
        editor.commands.addCommand({
            name: "exit",
            bindKey: { win: "esc", mac: "esc" },
            exec: function (editor) {
                if (confirm("确认退出")) {
                    window.closeLayer();
                }
            }
        });


    }).fail(function (error) {
        console.log(error);
    });
    ko.applyBindings(viewModel);
});

