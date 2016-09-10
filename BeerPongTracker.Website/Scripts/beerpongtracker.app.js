if (typeof BeerPongTracker !== "object") { BeerPongTracker = {}; }

BeerPongTracker.main = (function () {
    var _init = function () {
        BeerPongTracker.mainMenu.init();
        BeerPongTracker.helpers.init();
        BeerPongTracker.cupSelector.init();
        BeerPongTracker.startGameButton.init();

        _checkForExistingGame();
    };

    var _checkForExistingGame = function () {
        if (location.hash.match("^#c")) {
            BeerPongTracker.global.setControlling(true);
            var gameId = location.hash.replace("#c", "");
            BeerPongTracker.global.displayLoadingScreen();
            BeerPongTracker.global.loadGame(gameId);
        }

        if (location.hash.match("^#w")) {
            BeerPongTracker.global.setControlling(false);
            var gameId = location.hash.replace("#w", "");
            BeerPongTracker.global.displayLoadingScreen();
            BeerPongTracker.global.loadGame(gameId);
        }
    };

    return {
        init: _init
    };
})();

BeerPongTracker.global = (function () {
    var _controlling = false;

    var _isPc = $(window).width() > 1000;

    var _getIsPc = function () {
        return _isPc;
    };

    var _gameId = 0;

    var _numberOfTeams = 0;

    var _getNumberOfTeams = function () {
        return _numberOfTeams;
    };

    var _setNumberOfTeams = function (numberOfTeams) {
        _numberOfTeams = numberOfTeams;
    };

    var _getGameId = function () {
        return _gameId;
    };

    var _lastUpdateSignature = "";

    var _getLastUpdateSignature = function () {
        return _lastUpdateSignature;
    };

    var _setLastUpdateSignature = function (lastUpdateSignature) {
        _lastUpdateSignature = lastUpdateSignature;
    };

    var _setControlling = function (controllingBool) {
        _controlling = controllingBool;
    };

    var _getControlling = function () {
        return _controlling;
    };

    var _getGuid = function () {
        return Math.floor(Math.random() * 10000000000000001);
    }

    var _displayLoadingScreen = function () {
        $(".screen").hide();
        $("body").append("<div class=\"loading\"><img src=\"/Content/Images/loading.gif\" /></div>");
    };

    var _hideLoadingScreen = function () {
        $(".loading").remove();
    };

    var _loadGame = function (gameId) {
        _gameId = gameId;

        var jsonData = {
            GameId: gameId,
            Controlling: BeerPongTracker.global.getControlling(),
            IsPc: BeerPongTracker.global.getIsPc()
        };

        $.ajax({
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify(jsonData),
            success: _getGameDataSuccess,
            error: _getGameDataError,
            url: "/Game/Build"
        });
    }

    var _getGameDataSuccess = function (result) {
        $(".screen--3").append(result);
        _setLastUpdateSignature($(".game-meta-info").attr("lus"));
        _setNumberOfTeams($(".game-meta-info").attr("not"));
        if (BeerPongTracker.global.getControlling()) {
            BeerPongTracker.controlling.init();
        } else {
            BeerPongTracker.watching.init();
        }
        _hideLoadingScreen();
        $(".screen--3").show(result);
    };

    var _getGameDataError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    return {
        displayLoadingScreen: _displayLoadingScreen,
        hideLoadingScreen: _hideLoadingScreen,
        loadGame: _loadGame,
        setControlling: _setControlling,
        getControlling: _getControlling,
        getIsPc: _getIsPc,
        getGameId: _getGameId,
        getLastUpdateSignature: _getLastUpdateSignature,
        setLastUpdateSignature: _setLastUpdateSignature,
        getNumberOfTeams: _getNumberOfTeams,
        setNumberOfTeams: _setNumberOfTeams,
        getGuid: _getGuid
    };
})();

BeerPongTracker.mainMenu = (function () {
    var _init = function () {
        $(".button--watch-game").click(function () {
            BeerPongTracker.global.displayLoadingScreen();
            BeerPongTracker.global.setControlling(false);
            var gameId = $(this).attr("gameid");
            location.hash = '#w' + gameId;
            BeerPongTracker.global.loadGame(gameId);
        });

        $(".button--create-game").click(function () {
            $(".screen--1").hide();
            $(".screen--2").show();
        });
    };

    return {
        init: _init
    };
})();

BeerPongTracker.helpers = (function () {
    var _teamId = 0;
    var _playerId = 0;

    var _hidePlayerNameHelper = function () {
        if ($(".player-name-helper").length > 0) {
            $(".player-name-helper-overlay").remove();
            $(".player-name-helper").remove();
        }
    };

    var _helperOnTextbox = function (textBox) {
        if ($(textBox).val().length <= 2) {
            return;
        } else {
            var query = $(textBox).val();
            _teamId = $(textBox).attr("teamid");
            _playerId = $(textBox).attr("playerid");

            $.ajax({
                method: "GET",
                success: _playerNameHelperSuccess,
                error: _playerNameHelperError,
                url: "/Home/PlayerNameHelper?query=" + query + "&t=" + _teamId + "&p=" + _playerId
            });
        }
    };

    var _clearHidden = function (textBox) {
        var teamId = $(textBox).attr("teamid");
        var playerId = $(textBox).attr("playerid");

        var hiddenEl = _getHidden(playerId, teamId);

        $(hiddenEl).val("");
    };

    var _getHidden = function (playerId, teamId) {
        var pagePlayerIdTextBoxName = "team-input__text--hidden-db-player-id--team-" + teamId + "-player-" + playerId;
        return $("." + pagePlayerIdTextBoxName);
    };

    var _init = function () {
        $(".team-input__text").unbind().on("keyup", function () {
            _clearHidden(this);
            _helperOnTextbox(this);
        });

        $('.team-input__text-wrapper').on('click', '.player-name-helper-overlay', function () {
            _hidePlayerNameHelper();
        });

        $(".team-input__text").click(function () {
            _helperOnTextbox(this);
        });

        $('.team-input__text-wrapper').on('click', '.player-name-helper__table__row', function () {
            var dbPlayerId = $(this).attr("playerid");
            var dbPlayerName = $(this).attr("playername");

            var playerNameHelper = $(this).closest(".player-name-helper");

            var elPlayerId = $(playerNameHelper).attr("playerid");
            var elTeamId = $(playerNameHelper).attr("teamid");

            var pagePlayerNameTextBoxName = "team-input__text--team-" + elTeamId + "-player-" + elPlayerId;

            var pagePlayerNameTextBox = $("." + pagePlayerNameTextBoxName);

            $(pagePlayerNameTextBox).val(dbPlayerName);

            var hiddenEl = _getHidden(elPlayerId, elTeamId);

            $(hiddenEl).val(dbPlayerId);

            _hidePlayerNameHelper();
        });
    };

    var _playerNameHelperSuccess = function (result) {
        var elementName = "team-input__text--team-" + _teamId + "-player-" + _playerId + "-wrapper";

        _hidePlayerNameHelper();

        $("." + elementName).append(result);

        _teamId = 0;
        _playerId = 0;
    };

    var _playerNameHelperError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    return {
        init: _init
    };
})();

BeerPongTracker.cupSelector = (function () {
    var _init = function () {
        $(".number-of-cups-selector__selection").unbind().click(function () {
            _select(this);
        });
    };

    var _select = function (cupSelector) {
        $(".number-of-cups-selector__selection").each(function () {
            $(this).removeClass("number-of-cups-selector__selection--selected");
        });

        $(cupSelector).addClass("number-of-cups-selector__selection--selected");

        var numberOfCups = $(cupSelector).attr("cups");

        $(".number-of-cups-selector__hidden").val(numberOfCups);
    };

    return {
        init: _init
    };
})();

BeerPongTracker.controlling = (function () {
    var _init = function () {
        $(".cup-cover .cup").unbind().click(function () {
            if ($(this).hasClass("full")) {
                $(this).removeClass("full");
                $(this).addClass("empty");
            } else {
                $(this).removeClass("empty");
                $(this).addClass("full");
            }

            var cupId = $(this).attr('cupid');
            var teamId = $(this).attr('teamid');
            var gameId = $(this).parent(".cup-cover").attr('gameid');

            var jsonData =
            {
                GameId: gameId,
                TeamId: teamId,
                CupId: cupId,
            };

            $.ajax({
                contentType: "application/json",
                data: JSON.stringify(jsonData),
                method: "POST",
                success: _cupCoverSuccess,
                error: _cupCoverError,
                url: "/Game/CupSwitch"
            });
        });

        $("body").on("click", ".winner-prompt", function () {
            $(".winner-prompt").fadeOut();

            var winningTeamId = $(this).attr("teamid");

            BeerPongTracker.watching.declareWinner(winningTeamId);

            var jsonData = {
                WinningTeamId: winningTeamId,
                GameId: BeerPongTracker.global.getGameId()
            };

            $.ajax({
                contentType: "application/json",
                data: JSON.stringify(jsonData),
                method: "POST",
                success: function () { },
                error: function () { },
                url: "/Game/DeclareWinner"
            });
        });

        $("body").on("click", ".stop-music-button", function () {
            $(".stop-music-button").fadeOut();

            var gameId = BeerPongTracker.global.getGameId()
            var signature = "ew-" + BeerPongTracker.global.getGuid();

            $.ajax({
                contentType: "application/json",
                method: "GET",
                success: function () {
                    $(".screen--4").html("");
                    $(".screen--4").hide();
                    $(".screen--1").show();
                },
                error: function () { },
                url: "/Home/RegisterGenericEvent?sig=" + signature + "&gid=" + gameId
            });
        });

        var timeoutId = 0;

        $('.context-menu-button').mousedown(function () {
            timeoutId = setTimeout(_toggleContextMenu, 1000);
        }).bind('mouseup mouseleave', function () {
            clearTimeout(timeoutId);
        });

        var _toggleContextMenu = function () {
            if ($(".entrance-prompt").length > 0) {
                $(".entrance-prompt").remove();
                return;
            }

            var numberOfTeams = BeerPongTracker.global.getNumberOfTeams();

            for (teamId = 1; teamId <= numberOfTeams; teamId++) {
                var cupCoverElName = "cup-cover--team-" + teamId;
                var cupCoverEl = $("." + cupCoverElName);
                var positionStyle = _getCupCoverCentrePoint(BeerPongTracker.global.getNumberOfTeams(), teamId);
                cupCoverEl.prepend("<div state=\"1\" style=\"" + positionStyle + "\" class=\" button--green entrance-prompt\" teamid=\"" + teamId + "\"><span class=\"entrance-prompt--text\">PLAY ENTRANCE</span></div>");
            }
        };

        $("body").on("click", ".entrance-prompt", function () {
            if ($(this).attr("state") == "1") {
                $(this).removeClass("button--green");
                $(this).addClass("button--red");
                $(this).find(".entrance-prompt--text").html("STOP ENTRANCE");
                $(this).attr("state", "2");
                var gameId = BeerPongTracker.global.getGameId()
                var teamId = $(this).attr("teamid");
                var signature = "be-" + teamId + "-" + BeerPongTracker.global.getGuid();
                $.ajax({
                    contentType: "application/json",
                    method: "GET",
                    success: function () {
                    },
                    error: function () { },
                    url: "/Home/RegisterGenericEvent?sig=" + signature + "&gid=" + gameId
                });
            }
            else if ($(this).attr("state") == "2") {
                $(this).fadeOut();
                var gameId = BeerPongTracker.global.getGameId()
                var teamId = $(this).attr("teamid");
                var signature = "ee-" + teamId + "-" + BeerPongTracker.global.getGuid();
                $.ajax({
                    contentType: "application/json",
                    method: "GET",
                    success: function () {
                    },
                    error: function () { },
                    url: "/Home/RegisterGenericEvent?sig=" + signature + "&gid=" + gameId
                });
            }
        });
    };

    var _cupCoverSuccess = function (result) {
        if (BeerPongTracker.global.getIsPc()) {
            BeerPongTracker.watching.updateBoard();
        }
        _checkForWinner(result);
    }

    var _checkForWinner = function (gameModel) {
        var deadTeamsCount = 0;
        $(gameModel.Teams).each(function () {
            if (this.Health < 1) {
                deadTeamsCount++;
            }
        });
        if (deadTeamsCount == (gameModel.Teams.length - 1)) {
            var winningTeamId = 0;
            $(gameModel.Teams).each(function () {
                if (this.Health >= 1) {
                    winningTeamId = this.TeamId;
                    return _promptForEndGame(winningTeamId);
                }
            });
        } else {
            _removeEndGamePrompt();
        }
    }

    var _promptForEndGame = function (winningTeamId) {
        var cupCoverElName = "cup-cover--team-" + winningTeamId;
        var cupCoverEl = $("." + cupCoverElName);
        var positionStyle = _getCupCoverCentrePoint(BeerPongTracker.global.getNumberOfTeams(), winningTeamId);
        if ($(".winner-prompt").length == 0) {
            cupCoverEl.prepend("<div style=\"" + positionStyle + "\" class=\" button--gold winner-prompt\" teamid=\"" + winningTeamId + "\"><span class=\"winner-prompt--text\">DECLARE WINNER?</span></div>");
        }
    };

    var _removeEndGamePrompt = function () {
        if ($(".winner-prompt").length > 0) {
            $(".winner-prompt").remove();
        };
    };

    var _cupCoverError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    var _getCupCoverCentrePoint = function (numberOfTeams, teamId) {
        var cupCoverCentrePointMap = new Array();
        cupCoverCentrePointMap["2-1"] = "179@130";
        cupCoverCentrePointMap["2-2"] = "179@-192";
        cupCoverCentrePointMap["3-1"] = "229@213";
        cupCoverCentrePointMap["3-2"] = "225@499";
        cupCoverCentrePointMap["3-3"] = "477@352";
        cupCoverCentrePointMap["4-1"] = "301@134";
        cupCoverCentrePointMap["4-2"] = "121@293";
        cupCoverCentrePointMap["4-3"] = "291@468";
        cupCoverCentrePointMap["4-4"] = "470@294";

        var topLeft = cupCoverCentrePointMap[numberOfTeams + "-" + teamId];
        var topLeftArr = topLeft.split("@");

        return "top:" + (topLeftArr[0] - 70) + "px;left:" + (topLeftArr[1] - 70) + "px;"
    }

    return {
        init: _init
    };
})();

BeerPongTracker.watching = (function () {
    var _init = function () {
        var gameId = BeerPongTracker.global.getGameId;

        _listenForChange();
    };

    var _listenForChange = function () {
        var jsonData = {
            GameId: BeerPongTracker.global.getGameId(),
            LastUpdateSignature: BeerPongTracker.global.getLastUpdateSignature()
        };

        $.ajax({
            contentType: "application/json",
            data: JSON.stringify(jsonData),
            method: "POST",
            success: _listenForChangeSuccess,
            error: _listenForChangeError,
            url: "/Game/ListenForChange"
        });
    };

    var _listenForChangeSuccess = function (result) {
        if (result.Updated) {
            BeerPongTracker.global.setLastUpdateSignature(result.LastUpdateSignature);
            _resolveChangeNotice(result.LastUpdateSignature);
        }

        _listenForChange();
    };

    var _resolveChangeNotice = function (updateSignature) {
        if (updateSignature.match("^cs-")) {
            _updateBoard();
        };

        if (updateSignature.match("^ew-")) {
            _endDeclareWinner();
        };

        if (updateSignature.match("^tw-")) {
            var frags = updateSignature.split("-");
            var winningTeamId = frags[1];

            _declareWinner(winningTeamId);
        }

        if (updateSignature.match("^be-")) {
            var frags = updateSignature.split("-");
            var teamId = frags[1];

            _startEntrance(teamId);
        }

        if (updateSignature.match("^ee-")) {
            _endEntrance();
        }
    };

    var _startEntrance = function (teamId) {
        var gameId = BeerPongTracker.global.getGameId();
        $.ajax({
            contentType: "application/json",
            method: "GET",
            success: function (result) {
                $(".screen--entrance").html(result);
                BeerPongTracker.entrance.init();
                $(".screen--3").hide();
                $(".screen--entrance").show();
            },
            error: function () { },
            url: "/Home/GetEntranceScreen?gameId=" + gameId + "&teamId=" + teamId
        });
    };

    var _endEntrance = function () {
        $(".screen--entrance").html("");
        $(".screen--entrance").hide();
        $(".screen--3").show();
    };

    var _declareWinner = function (winningTeamId) {
        var jsonData = {
            WinningTeamId: winningTeamId,
            GameId: BeerPongTracker.global.getGameId(),
            Controlling: BeerPongTracker.global.getControlling()
        };

        $.ajax({
            contentType: "application/json",
            data: JSON.stringify(jsonData),
            method: "POST",
            success: _declareWinnerSuccess,
            error: _declareWinnerError,
            url: "/Home/GetWinnerDetails"
        });
    };

    var _declareWinnerSuccess = function (result) {
        $(".screen--4").html(result);
        $(".screen--3").fadeOut(3000, function () {
            setTimeout(function () {
                $(".screen--4").fadeIn(1000);
                BeerPongTracker.winScreen.init();
            }, 3000);
        });
    };

    var _endDeclareWinner = function () {
        $(".screen--4").html("");
        $(".screen--4").hide();
        $(".screen--3").show();
    }

    var _declareWinnerError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    var _updateBoard = function () {
        $.ajax({
            contentType: "application/json",
            method: "GET",
            success: _updateBoardSuccess,
            error: _updateBoardError,
            url: "/Game/GetGame?gameId=" + BeerPongTracker.global.getGameId()
        });
    }

    var _updateBoardSuccess = function (result) {
        $(result.Teams).each(function () {
            var teamId = this.TeamId;
            var teamHealthElement = $("#team-" + this.TeamId + "-health");
            teamHealthElement.css({ width: this.Health + "%" });

            if (this.Health < 25) {
                teamHealthElement.removeClass("team-info__health__container__remaining--green");
                teamHealthElement.addClass("team-info__health__container__remaining--red");
            } else {
                teamHealthElement.removeClass("team-info__health__container__remaining--red");
                teamHealthElement.addClass("team-info__health__container__remaining--green");
            }

            $(this.CupStats).each(function () {
                var cupElementId = "team-" + teamId + "-cup-" + this.CupId;
                var cupElement = $("#" + cupElementId);
                if (this.Active) {
                    cupElement.removeClass("empty");
                    cupElement.addClass("full");
                } else {
                    cupElement.removeClass("full");
                    cupElement.addClass("empty");
                }
            });
        });
    };

    var _updateBoardError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    var _listenForChangeError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    return {
        init: _init,
        updateBoard: _updateBoard,
        declareWinner: _declareWinner,
        endDeclareWinner: _endDeclareWinner
    };
})();

BeerPongTracker.startGameButton = (function () {
    var _init = function () {
        $(".button--start-game").unbind().click(function () {
            BeerPongTracker.global.displayLoadingScreen();

            var numberOfCups = $(".number-of-cups-selector__hidden").val();

            var teams = new Array();

            for (i = 1; i <= 4; i++) {
                var topTextBoxName = "team-input__text--team-" + i + "-player-1";

                if ($("." + topTextBoxName).val().length > 0) {
                    teamIndex = i - 1;
                    teams[teamIndex] = { Players: new Array() };

                    var teamGroupName = "team-input__text--team-" + i;

                    $("." + teamGroupName).each(function () {
                        var playerId = $(this).attr("playerid");
                        var playerIndex = playerId - 1;

                        var hiddenElName = "team-input__text--hidden-db-player-id--team-" + i + "-player-" + playerId
                        var hiddenEl = $("." + hiddenElName);

                        if ($(hiddenEl).val() > 0) {
                            var dbPlayerId = $(hiddenEl).val();
                            teams[teamIndex].Players[playerIndex] = { PlayerId: dbPlayerId }
                        } else if ($(this).val().length > 0) {
                            teams[teamIndex].Players[playerIndex] = { Name: $(this).val() }
                        }
                    });
                }
            }

            var jsonData = {
                NumberOfCups: numberOfCups,
                Teams: teams
            }

            $.ajax({
                contentType: "application/json",
                data: JSON.stringify(jsonData),
                method: "POST",
                success: _startGameSuccess,
                error: _startGameError,
                url: "/Game/StartGame"
            });
        });
    };

    var _startGameSuccess = function (result) {
        var gameId = result.GameId;

        location.hash = '#c' + gameId;

        BeerPongTracker.global.setControlling(true);

        BeerPongTracker.global.loadGame(gameId);
    };

    var _startGameError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown)
    };

    return {
        init: _init
    };
})();

BeerPongTracker.winScreen = (function () {
    var _init = function () {
        setInterval(function () { _flash() }, 250);

        setInterval(function () { _flash2() }, 2000);

        setTimeout(function () {
            $(".name-plate-view-wrapper").show();
            $(".name-plate-view-wrapper").addClass("flipInX animated");
        }, 7000);

        BeerPongTracker.confetti.init(false);
    };

    var _flash = function () {
        var flashingEl = $(".screen--fixed--outer");
        if (flashingEl.hasClass("orange-background")) {
            flashingEl.removeClass("orange-background");
            flashingEl.addClass("orange2-background");
        } else {
            flashingEl.addClass("orange-background");
            flashingEl.removeClass("orange2-background");
        }
    };

    var _flash2 = function () {
        setTimeout(function () {
            var flashingEl = $(".screen--fixed--inner");
            flashingEl.addClass("camera-flash");
            setTimeout(function () {
                flashingEl.removeClass("camera-flash");
            }, 100);
        }, Math.floor(Math.random() * 2000) + 1);
    };

    return {
        init: _init
    };
})();

BeerPongTracker.entrance = (function () {
    var _switch = 0;

    var _screenEl;

    var _textEl;

    var _imageEl;

    var flashInterval;

    var animationInterval;

    var _init = function () {
        clearInterval(flashInterval);
        clearInterval(animationInterval);

        var _flash = function (palette) {
            flashInterval = setInterval(function () {
                if (_switch == 0) {
                    _switch = 1;
                    $(_screenEl).attr("style", "background:#" + palette.backBase + ";");
                } else {
                    _switch = 0;
                    $(_screenEl).attr("style", "background:#" + palette.backLight + ";");
                }
            }, flashRate);
        };

        var _flash2 = function (palette, font) {
            flashInterval = setInterval(function () {
                if (_switch == 0) {
                    _switch = 1;
                    $(_screenEl).attr("style", "background:#" + palette.backBase + ";");
                    $(".screen--entrance__name").attr("style", "color:#" + palette.textBase + ";font-family:" + font + ";");
                } else {
                    _switch = 0;
                    $(_screenEl).attr("style", "background:#" + palette.textBase + ";");
                    $(".screen--entrance__name").attr("style", "color:#" + palette.backBase + ";font-family:" + font + ";");
                }
            }, flashRate);
        };

        var _colorText = function (palette, font) {
            $(".screen--entrance__name").attr("style", "color:#" + palette.textBase + ";font-family:" + font + ";");
        }

        var _animation1 = function (palette, textAnimation, imageAnimation, flashRate, font) {
            var _animation1Switch = 0;

            _flash(palette);
            _colorText(palette, font);

            $(_textEl).removeClass("hide");
            $(_textEl).addClass(textAnimation.fadeIn);

            animationInterval = setInterval(function () {
                if (_animation1Switch == 0) {
                    _animation1Switch = 1;
                    $(_textEl).addClass(textAnimation.fadeOut);
                    $(_imageEl).fadeIn(500);
                } else {
                    _animation1Switch = 0;
                    $(_imageEl).fadeOut(500);
                    $(_textEl).removeClass(textAnimation.fadeOut);
                    $(_textEl).addClass(textAnimation.fadeIn);
                    _animation1Switch = 0;
                }
            }, 5000);
        }

        var _animation2 = function (palette, textAnimation, imageAnimation, flashRate, font) {
            var _animation1Switch = 0;

            _colorText(palette, font);

            _flash2(palette, font);

            $(_textEl).removeClass("hide");
            $(_textEl).addClass(textAnimation.fadeIn);

            animationInterval = setInterval(function () {
                if (_animation1Switch == 0) {
                    _animation1Switch = 1;
                    $(_textEl).addClass(textAnimation.fadeOut);
                    $(_imageEl).fadeIn(500);
                } else {
                    _animation1Switch = 0;
                    $(_imageEl).fadeOut(500);
                    $(_textEl).removeClass(textAnimation.fadeOut);
                    $(_textEl).addClass(textAnimation.fadeIn);
                    _animation1Switch = 0;
                }
            }, 5000);
        }

        var palettes = new Array();
        palettes.push({
            backBase: "1f1f1f",
            backLight: "626262",
            textBase: "fd0d0c",
        });
        palettes.push({
            backBase: "1f1f1f",
            backLight: "626262",
            textBase: "fee336",
        });
        palettes.push({
            backBase: "1f1f1f",
            backLight: "333333",
            textBase: "a2449c",
        });
        palettes.push({
            backBase: "1f1f1f",
            backLight: "626262",
            textBase: "3eaa3b",
        });
        palettes.push({
            backBase: "2a465c",
            backLight: "697d8d",
            textBase: "fffffd",
        });
        palettes.push({
            backBase: "0f398f",
            backLight: "5774b0",
            textBase: "fffffd",
        });
        palettes.push({
            backBase: "c80e0f",
            backLight: "d85657",
            textBase: "fffffd",
        });
        palettes.push({
            backBase: "8fbed2",
            backLight: "b0d1df",
            textBase: "f8a145",
        });
        palettes.push({
            backBase: "1d3676",
            backLight: "60729f",
            textBase: "fffffd",
        });
        palettes.push({
            backBase: "fce5a0",
            backLight: "fdedbc",
            textBase: "fec401",
        });
        palettes.push({
            backBase: "0aacec",
            backLight: "53c5f2",
            textBase: "fffffd",
        });
        palettes.push({
            backBase: "014fb3",
            backLight: "4d83ca",
            textBase: "fecf23",
        });
        palettes.push({
            backBase: "7b5ab3",
            backLight: "a18ac9",
            textBase: "fff101",
        });
        palettes.push({
            backBase: "d20423",
            backLight: "df4d63",
            textBase: "3bd432",
        });
        palettes.push({
            backBase: "2d282e",
            backLight: "6a666b",
            textBase: "f17305",
        });
        palettes.push({
            backBase: "0054b6",
            backLight: "4a86cb",
            textBase: "faffff",
        });
        palettes.push({
            backBase: "0cc8c2",
            backLight: "53d8d4",
            textBase: "804b1b",
        });
        palettes.push({
            backBase: "c40010",
            backLight: "d54a55",
            textBase: "030204",
        });
        palettes.push({
            backBase: "fd5636",
            backLight: "fe8770",
            textBase: "423434",
        });
        palettes.push({
            backBase: "c261a5",
            backLight: "d48fbf",
            textBase: "180d17",
        });


        var textAnimations = new Array();
        textAnimations.push({
            fadeIn: "zoomIn",
            fadeOut: "zoomOut",
        });
        textAnimations.push({
            fadeIn: "slideInLeft",
            fadeOut: "slideOutRight",
        });
        textAnimations.push({
            fadeIn: "rotateIn",
            fadeOut: "rotateOut",
        });
        textAnimations.push({
            fadeIn: "bounceIn",
            fadeOut: "bounceOut",
        });
        textAnimations.push({
            fadeIn: "flipInY",
            fadeOut: "flipOutY",
        });
        textAnimations.push({
            fadeIn: "lightSpeedIn",
            fadeOut: "lightSpeedOut",
        });
        textAnimations.push({
            fadeIn: "rollIn",
            fadeOut: "rollOut",
        });
        textAnimations.push({
            fadeIn: "fadeInDown",
            fadeOut: "fadeOutDown",
        });
        textAnimations.push({
            fadeIn: "bounceInLeft",
            fadeOut: "bounceOutRight",
        });
        textAnimations.push({
            fadeIn: "zoomInUp",
            fadeOut: "zoomOutDown",
        });

        var fonts = new Array("Baloo Chettan", "Amatic SC", "Pacifico", "Patua One", "Bangers", "Black Ops One", "Monoton", "Audiowide", "Special Elite", "Bungee Inline", "Bubblegum Sans", "Fontdiner Swanky", "Corben", "Cinzel Decorative", "Bowlby One SC", "Expletus Sans", "Rammetto One", "Faster One", "Henny Penny", "Irish Grover");

        var palette = palettes[(1 + Math.floor(Math.random() * palettes.length)) - 1];

        var textAnimation = textAnimations[(1 + Math.floor(Math.random() * textAnimations.length)) - 1];

        var imageAnimation = textAnimations[(1 + Math.floor(Math.random() * textAnimations.length)) - 1];

        var font = fonts[(1 + Math.floor(Math.random() * fonts.length)) - 1];

        var flashRate = (2 + (1 + Math.floor(Math.random() * 5))) * 70;

        _screenEl = $(".screen--entrance");

        _textEl = $(".screen--entrance__name");

        _imageEl = $(".screen--entrance__picture");

        var animationId = 1 + Math.floor(Math.random() * 2);

        if (animationId == 1) {
            _animation1(palette, textAnimation, imageAnimation, flashRate, font);
        } else {
            _animation2(palette, textAnimation, imageAnimation, flashRate, font);
        }

        if (Math.floor((Math.random() * 2) + 1) == 1) {
            console.log("confetti on");
            setTimeout(function () {
                BeerPongTracker.confetti.init(true);
            }, 5000);
        } else {
            console.log("confetti off");
        }
    };

    return {
        init: _init
    };
})();

BeerPongTracker.confetti = (function () {
    var _init = function (streamersOnly) {
        var retina = window.devicePixelRatio,

				// Math shorthands
				PI = Math.PI,
				sqrt = Math.sqrt,
				round = Math.round,
				random = Math.random,
				cos = Math.cos,
				sin = Math.sin,

				// Local WindowAnimationTiming interface
				rAF = window.requestAnimationFrame,
				cAF = window.cancelAnimationFrame || window.cancelRequestAnimationFrame;

        // Local WindowAnimationTiming interface polyfill
        (function (w) {
            /**
            * Fallback implementation.
            */
            var prev = new Date().getTime();
            function fallback(fn) {
                var curr = _now();
                var ms = Math.max(0, 16 - (curr - prev));
                var req = setTimeout(fn, ms);
                prev = curr;
                return req;
            }

            /**
            * Cancel.
            */
            var cancel = w.cancelAnimationFrame
                || w.webkitCancelAnimationFrame
                || w.clearTimeout;

            rAF = w.requestAnimationFrame
                || w.webkitRequestAnimationFrame
                || fallback;

            cAF = function (id) {
                cancel.call(w, id);
            };
        }(window));
        
        var confettiPaperCount = 95;

        if (streamersOnly) {
            confettiPaperCount = 0;
        }

        var speed = 20,
            duration = (1.0 / speed),
            confettiRibbonCount = 25,
            ribbonPaperCount = 30,
            ribbonPaperDist = 8.0,
            ribbonPaperThick = 8.0,
            DEG_TO_RAD = PI / 180,
            RAD_TO_DEG = 180 / PI,
            colors = [
                ["#df0049", "#660671"],
                ["#00e857", "#005291"],
                ["#2bebbc", "#05798a"],
                ["#ffd200", "#b06c00"]
            ];

        function Vector2(_x, _y) {
            this.x = _x, this.y = _y;
            this.Length = function () {
                return sqrt(this.SqrLength());
            }
            this.SqrLength = function () {
                return this.x * this.x + this.y * this.y;
            }
            this.Add = function (_vec) {
                this.x += _vec.x;
                this.y += _vec.y;
            }
            this.Sub = function (_vec) {
                this.x -= _vec.x;
                this.y -= _vec.y;
            }
            this.Div = function (_f) {
                this.x /= _f;
                this.y /= _f;
            }
            this.Mul = function (_f) {
                this.x *= _f;
                this.y *= _f;
            }
            this.Normalize = function () {
                var sqrLen = this.SqrLength();
                if (sqrLen != 0) {
                    var factor = 1.0 / sqrt(sqrLen);
                    this.x *= factor;
                    this.y *= factor;
                }
            }
            this.Normalized = function () {
                var sqrLen = this.SqrLength();
                if (sqrLen != 0) {
                    var factor = 1.0 / sqrt(sqrLen);
                    return new Vector2(this.x * factor, this.y * factor);
                }
                return new Vector2(0, 0);
            }
        }
        Vector2.Lerp = function (_vec0, _vec1, _t) {
            return new Vector2((_vec1.x - _vec0.x) * _t + _vec0.x, (_vec1.y - _vec0.y) * _t + _vec0.y);
        }
        Vector2.Distance = function (_vec0, _vec1) {
            return sqrt(Vector2.SqrDistance(_vec0, _vec1));
        }
        Vector2.SqrDistance = function (_vec0, _vec1) {
            var x = _vec0.x - _vec1.x;
            var y = _vec0.y - _vec1.y;
            return (x * x + y * y + z * z);
        }
        Vector2.Scale = function (_vec0, _vec1) {
            return new Vector2(_vec0.x * _vec1.x, _vec0.y * _vec1.y);
        }
        Vector2.Min = function (_vec0, _vec1) {
            return new Vector2(Math.min(_vec0.x, _vec1.x), Math.min(_vec0.y, _vec1.y));
        }
        Vector2.Max = function (_vec0, _vec1) {
            return new Vector2(Math.max(_vec0.x, _vec1.x), Math.max(_vec0.y, _vec1.y));
        }
        Vector2.ClampMagnitude = function (_vec0, _len) {
            var vecNorm = _vec0.Normalized;
            return new Vector2(vecNorm.x * _len, vecNorm.y * _len);
        }
        Vector2.Sub = function (_vec0, _vec1) {
            return new Vector2(_vec0.x - _vec1.x, _vec0.y - _vec1.y, _vec0.z - _vec1.z);
        }

        function EulerMass(_x, _y, _mass, _drag) {
            this.position = new Vector2(_x, _y);
            this.mass = _mass;
            this.drag = _drag;
            this.force = new Vector2(0, 0);
            this.velocity = new Vector2(0, 0);
            this.AddForce = function (_f) {
                this.force.Add(_f);
            }
            this.Integrate = function (_dt) {
                var acc = this.CurrentForce(this.position);
                acc.Div(this.mass);
                var posDelta = new Vector2(this.velocity.x, this.velocity.y);
                posDelta.Mul(_dt);
                this.position.Add(posDelta);
                acc.Mul(_dt);
                this.velocity.Add(acc);
                this.force = new Vector2(0, 0);
            }
            this.CurrentForce = function (_pos, _vel) {
                var totalForce = new Vector2(this.force.x, this.force.y);
                var speed = this.velocity.Length();
                var dragVel = new Vector2(this.velocity.x, this.velocity.y);
                dragVel.Mul(this.drag * this.mass * speed);
                totalForce.Sub(dragVel);
                return totalForce;
            }
        }

        function ConfettiPaper(_x, _y) {
            this.pos = new Vector2(_x, _y);
            this.rotationSpeed = (random() * 600 + 800);
            this.angle = DEG_TO_RAD * random() * 360;
            this.rotation = DEG_TO_RAD * random() * 360;
            this.cosA = 1.0;
            this.size = 5.0;
            this.oscillationSpeed = (random() * 1.5 + 0.5);
            this.xSpeed = 40.0;
            this.ySpeed = (random() * 60 + 50.0);
            this.corners = new Array();
            this.time = random();
            var ci = round(random() * (colors.length - 1));
            this.frontColor = colors[ci][0];
            this.backColor = colors[ci][1];
            for (var i = 0; i < 4; i++) {
                var dx = cos(this.angle + DEG_TO_RAD * (i * 90 + 45));
                var dy = sin(this.angle + DEG_TO_RAD * (i * 90 + 45));
                this.corners[i] = new Vector2(dx, dy);
            }
            this.Update = function (_dt) {
                this.time += _dt;
                this.rotation += this.rotationSpeed * _dt;
                this.cosA = cos(DEG_TO_RAD * this.rotation);
                this.pos.x += cos(this.time * this.oscillationSpeed) * this.xSpeed * _dt
                this.pos.y += this.ySpeed * _dt;
                if (this.pos.y > ConfettiPaper.bounds.y) {
                    this.pos.x = random() * ConfettiPaper.bounds.x;
                    this.pos.y = 0;
                }
            }
            this.Draw = function (_g) {
                if (this.cosA > 0) {
                    _g.fillStyle = this.frontColor;
                } else {
                    _g.fillStyle = this.backColor;
                }
                _g.beginPath();
                _g.moveTo((this.pos.x + this.corners[0].x * this.size) * retina, (this.pos.y + this.corners[0].y * this.size * this.cosA) * retina);
                for (var i = 1; i < 4; i++) {
                    _g.lineTo((this.pos.x + this.corners[i].x * this.size) * retina, (this.pos.y + this.corners[i].y * this.size * this.cosA) * retina);
                }
                _g.closePath();
                _g.fill();
            }
        }
        ConfettiPaper.bounds = new Vector2(0, 0);

        function ConfettiRibbon(_x, _y, _count, _dist, _thickness, _angle, _mass, _drag) {
            this.particleDist = _dist;
            this.particleCount = _count;
            this.particleMass = _mass;
            this.particleDrag = _drag;
            this.particles = new Array();
            var ci = round(random() * (colors.length - 1));
            this.frontColor = colors[ci][0];
            this.backColor = colors[ci][1];
            this.xOff = (cos(DEG_TO_RAD * _angle) * _thickness);
            this.yOff = (sin(DEG_TO_RAD * _angle) * _thickness);
            this.position = new Vector2(_x, _y);
            this.prevPosition = new Vector2(_x, _y);
            this.velocityInherit = (random() * 2 + 4);
            this.time = random() * 100;
            this.oscillationSpeed = (random() * 2 + 2);
            this.oscillationDistance = (random() * 40 + 40);
            this.ySpeed = (random() * 40 + 80);
            for (var i = 0; i < this.particleCount; i++) {
                this.particles[i] = new EulerMass(_x, _y - i * this.particleDist, this.particleMass, this.particleDrag);
            }
            this.Update = function (_dt) {
                var i = 0;
                this.time += _dt * this.oscillationSpeed;
                this.position.y += this.ySpeed * _dt;
                this.position.x += cos(this.time) * this.oscillationDistance * _dt;
                this.particles[0].position = this.position;
                var dX = this.prevPosition.x - this.position.x;
                var dY = this.prevPosition.y - this.position.y;
                var delta = sqrt(dX * dX + dY * dY);
                this.prevPosition = new Vector2(this.position.x, this.position.y);
                for (i = 1; i < this.particleCount; i++) {
                    var dirP = Vector2.Sub(this.particles[i - 1].position, this.particles[i].position);
                    dirP.Normalize();
                    dirP.Mul((delta / _dt) * this.velocityInherit);
                    this.particles[i].AddForce(dirP);
                }
                for (i = 1; i < this.particleCount; i++) {
                    this.particles[i].Integrate(_dt);
                }
                for (i = 1; i < this.particleCount; i++) {
                    var rp2 = new Vector2(this.particles[i].position.x, this.particles[i].position.y);
                    rp2.Sub(this.particles[i - 1].position);
                    rp2.Normalize();
                    rp2.Mul(this.particleDist);
                    rp2.Add(this.particles[i - 1].position);
                    this.particles[i].position = rp2;
                }
                if (this.position.y > ConfettiRibbon.bounds.y + this.particleDist * this.particleCount) {
                    this.Reset();
                }
            }
            this.Reset = function () {
                this.position.y = -random() * ConfettiRibbon.bounds.y;
                this.position.x = random() * ConfettiRibbon.bounds.x;
                this.prevPosition = new Vector2(this.position.x, this.position.y);
                this.velocityInherit = random() * 2 + 4;
                this.time = random() * 100;
                this.oscillationSpeed = random() * 2.0 + 1.5;
                this.oscillationDistance = (random() * 40 + 40);
                this.ySpeed = random() * 40 + 80;
                var ci = round(random() * (colors.length - 1));
                this.frontColor = colors[ci][0];
                this.backColor = colors[ci][1];
                this.particles = new Array();
                for (var i = 0; i < this.particleCount; i++) {
                    this.particles[i] = new EulerMass(this.position.x, this.position.y - i * this.particleDist, this.particleMass, this.particleDrag);
                }
            }
            this.Draw = function (_g) {
                for (var i = 0; i < this.particleCount - 1; i++) {
                    var p0 = new Vector2(this.particles[i].position.x + this.xOff, this.particles[i].position.y + this.yOff);
                    var p1 = new Vector2(this.particles[i + 1].position.x + this.xOff, this.particles[i + 1].position.y + this.yOff);
                    if (this.Side(this.particles[i].position.x, this.particles[i].position.y, this.particles[i + 1].position.x, this.particles[i + 1].position.y, p1.x, p1.y) < 0) {
                        _g.fillStyle = this.frontColor;
                        _g.strokeStyle = this.frontColor;
                    } else {
                        _g.fillStyle = this.backColor;
                        _g.strokeStyle = this.backColor;
                    }
                    if (i == 0) {
                        _g.beginPath();
                        _g.moveTo(this.particles[i].position.x * retina, this.particles[i].position.y * retina);
                        _g.lineTo(this.particles[i + 1].position.x * retina, this.particles[i + 1].position.y * retina);
                        _g.lineTo(((this.particles[i + 1].position.x + p1.x) * 0.5) * retina, ((this.particles[i + 1].position.y + p1.y) * 0.5) * retina);
                        _g.closePath();
                        _g.stroke();
                        _g.fill();
                        _g.beginPath();
                        _g.moveTo(p1.x * retina, p1.y * retina);
                        _g.lineTo(p0.x * retina, p0.y * retina);
                        _g.lineTo(((this.particles[i + 1].position.x + p1.x) * 0.5) * retina, ((this.particles[i + 1].position.y + p1.y) * 0.5) * retina);
                        _g.closePath();
                        _g.stroke();
                        _g.fill();
                    } else if (i == this.particleCount - 2) {
                        _g.beginPath();
                        _g.moveTo(this.particles[i].position.x * retina, this.particles[i].position.y * retina);
                        _g.lineTo(this.particles[i + 1].position.x * retina, this.particles[i + 1].position.y * retina);
                        _g.lineTo(((this.particles[i].position.x + p0.x) * 0.5) * retina, ((this.particles[i].position.y + p0.y) * 0.5) * retina);
                        _g.closePath();
                        _g.stroke();
                        _g.fill();
                        _g.beginPath();
                        _g.moveTo(p1.x * retina, p1.y * retina);
                        _g.lineTo(p0.x * retina, p0.y * retina);
                        _g.lineTo(((this.particles[i].position.x + p0.x) * 0.5) * retina, ((this.particles[i].position.y + p0.y) * 0.5) * retina);
                        _g.closePath();
                        _g.stroke();
                        _g.fill();
                    } else {
                        _g.beginPath();
                        _g.moveTo(this.particles[i].position.x * retina, this.particles[i].position.y * retina);
                        _g.lineTo(this.particles[i + 1].position.x * retina, this.particles[i + 1].position.y * retina);
                        _g.lineTo(p1.x * retina, p1.y * retina);
                        _g.lineTo(p0.x * retina, p0.y * retina);
                        _g.closePath();
                        _g.stroke();
                        _g.fill();
                    }
                }
            }
            this.Side = function (x1, y1, x2, y2, x3, y3) {
                return ((x1 - x2) * (y3 - y2) - (y1 - y2) * (x3 - x2));
            }
        }
        ConfettiRibbon.bounds = new Vector2(0, 0);
        confetti = {};
        confetti.Context = function (id) {
            var i = 0;
            var canvas = document.getElementById(id);
            var canvasParent = canvas.parentNode;
            var canvasWidth = $(window).width();
            var canvasHeight = $(window).height();
            canvas.width = canvasWidth * retina;
            canvas.height = canvasHeight * retina;
            var context = canvas.getContext('2d');
            var interval = null;
            var confettiRibbons = new Array();
            ConfettiRibbon.bounds = new Vector2(canvasWidth, canvasHeight);
            for (i = 0; i < confettiRibbonCount; i++) {
                confettiRibbons[i] = new ConfettiRibbon(random() * canvasWidth, -random() * canvasHeight * 2, ribbonPaperCount, ribbonPaperDist, ribbonPaperThick, 45, 1, 0.05);
            }
            var confettiPapers = new Array();
            ConfettiPaper.bounds = new Vector2(canvasWidth, canvasHeight);
            for (i = 0; i < confettiPaperCount; i++) {
                confettiPapers[i] = new ConfettiPaper(random() * canvasWidth, random() * canvasHeight);
            }
            this.resize = function () {
                canvasWidth = $(window).width();
                canvasHeight = $(window).height();
                canvas.width = canvasWidth * retina;
                canvas.height = canvasHeight * retina;
                ConfettiPaper.bounds = new Vector2(canvasWidth, canvasHeight);
                ConfettiRibbon.bounds = new Vector2(canvasWidth, canvasHeight);
            }
            this.start = function () {
                this.stop()
                var context = this;
                this.update();
            }
            this.stop = function () {
                cAF(this.interval);
            }
            this.update = function () {
                var i = 0;
                context.clearRect(0, 0, canvas.width, canvas.height);
                for (i = 0; i < confettiPaperCount; i++) {
                    confettiPapers[i].Update(duration);
                    confettiPapers[i].Draw(context);
                }
                for (i = 0; i < confettiRibbonCount; i++) {
                    confettiRibbons[i].Update(duration);
                    confettiRibbons[i].Draw(context);
                }
                this.interval = rAF(function () {
                    confetti.update();
                });
            }
        }
        var confetti = new confetti.Context('confetti');
        
        confetti.start();
        window.addEventListener('resize', function (event) {
            confetti.resize();
        });
    };

    return {
        init: _init
    };
})();