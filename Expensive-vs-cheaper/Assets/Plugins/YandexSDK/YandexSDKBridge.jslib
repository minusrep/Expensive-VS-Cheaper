var DoubleBYandexSDKBridge = {
    sdkUrl: 'https://sdk.games.s3.yandex.net/sdk.js',
    ysdk: null,
    player: null,
    payments: null,
    initPromise: null,

    toString: function (ptr) {
        return ptr ? UTF8ToString(ptr) : '';
    },

    toPtr: function (value) {
        var text = value == null ? '' : String(value);
        var length = lengthBytesUTF8(text) + 1;
        var buffer = _malloc(length);
        stringToUTF8(text, buffer, length);
        return buffer;
    },

    send: function (gameObjectName, callbackId, success, payload, error) {
        var message = JSON.stringify({
            id: callbackId,
            success: !!success,
            payload: payload == null ? '' : String(payload),
            error: error == null ? '' : String(error)
        });

        var unity = null;
        if (typeof unityInstance !== 'undefined' && unityInstance) {
            unity = unityInstance;
        } else if (typeof window !== 'undefined' && window.unityInstance) {
            unity = window.unityInstance;
        } else if (typeof Module !== 'undefined' && Module && Module.SendMessage) {
            unity = Module;
        }

        if (unity && unity.SendMessage) {
            unity.SendMessage(gameObjectName, 'HandleYandexSDKCallback', message);
        } else if (typeof SendMessage !== 'undefined') {
            SendMessage(gameObjectName, 'HandleYandexSDKCallback', message);
        } else {
            console.error('[YandexSDKBridge] Unity SendMessage is unavailable.', message);
        }
    },

    errorToString: function (error) {
        if (!error) {
            return 'Unknown error';
        }

        if (typeof error === 'string') {
            return error;
        }

        if (error.message) {
            return error.message;
        }

        try {
            return JSON.stringify(error);
        } catch (e) {
            return String(error);
        }
    },

    safeJson: function (value) {
        if (value == null) {
            return '';
        }

        if (typeof value === 'string') {
            return value;
        }

        try {
            return JSON.stringify(value);
        } catch (e) {
            return String(value);
        }
    },

    parseJson: function (json, fallback) {
        if (!json) {
            return fallback;
        }

        try {
            return JSON.parse(json);
        } catch (e) {
            console.warn('[YandexSDKBridge] Invalid JSON:', json, e);
            return fallback;
        }
    },

    loadScript: function () {
        if (typeof YaGames !== 'undefined') {
            return Promise.resolve();
        }

        if (typeof document === 'undefined') {
            return Promise.reject('Document is unavailable.');
        }

        var existing = document.querySelector('script[src="' + DoubleBYandexSDKBridge.sdkUrl + '"]');
        if (existing) {
            return new Promise(function (resolve, reject) {
                if (typeof YaGames !== 'undefined') {
                    resolve();
                    return;
                }

                existing.addEventListener('load', function () { resolve(); });
                existing.addEventListener('error', function () { reject('Failed to load Yandex Games SDK.'); });
            });
        }

        return new Promise(function (resolve, reject) {
            var script = document.createElement('script');
            script.src = DoubleBYandexSDKBridge.sdkUrl;
            script.async = true;
            script.onload = function () { resolve(); };
            script.onerror = function () { reject('Failed to load Yandex Games SDK.'); };
            document.head.appendChild(script);
        });
    },

    init: function () {
        if (DoubleBYandexSDKBridge.initPromise) {
            return DoubleBYandexSDKBridge.initPromise;
        }

        DoubleBYandexSDKBridge.initPromise = DoubleBYandexSDKBridge.loadScript()
            .then(function () {
                if (typeof YaGames === 'undefined') {
                    throw new Error('YaGames is unavailable after SDK script load.');
                }

                return YaGames.init();
            })
            .then(function (ysdk) {
                DoubleBYandexSDKBridge.ysdk = ysdk;
                return ysdk;
            });

        return DoubleBYandexSDKBridge.initPromise;
    },

    requireSdk: function () {
        return DoubleBYandexSDKBridge.ysdk
            ? Promise.resolve(DoubleBYandexSDKBridge.ysdk)
            : DoubleBYandexSDKBridge.init();
    },

    requirePlayer: function () {
        return DoubleBYandexSDKBridge.requireSdk()
            .then(function (ysdk) {
                if (DoubleBYandexSDKBridge.player) {
                    return DoubleBYandexSDKBridge.player;
                }

                return ysdk.getPlayer().then(function (player) {
                    DoubleBYandexSDKBridge.player = player;
                    return player;
                });
            });
    },

    requirePayments: function () {
        return DoubleBYandexSDKBridge.requireSdk()
            .then(function (ysdk) {
                if (DoubleBYandexSDKBridge.payments) {
                    return DoubleBYandexSDKBridge.payments;
                }

                if (ysdk.payments) {
                    DoubleBYandexSDKBridge.payments = ysdk.payments;
                    return ysdk.payments;
                }

                return ysdk.getPayments().then(function (payments) {
                    DoubleBYandexSDKBridge.payments = payments;
                    return payments;
                });
            });
    },

    complete: function (gameObjectName, callbackId, promise) {
        promise
            .then(function (payload) {
                DoubleBYandexSDKBridge.send(gameObjectName, callbackId, true, DoubleBYandexSDKBridge.safeJson(payload), '');
            })
            .catch(function (error) {
                DoubleBYandexSDKBridge.send(gameObjectName, callbackId, false, '', DoubleBYandexSDKBridge.errorToString(error));
            });
    }
};

mergeInto(LibraryManager.library, {
    YandexSDK_IsInitialized: function () {
        return DoubleBYandexSDKBridge.ysdk ? 1 : 0;
    },

    YandexSDK_Init: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId, DoubleBYandexSDKBridge.init().then(function () {
            return { initialized: true };
        }));
    },

    YandexSDK_GetEnvironment: function () {
        var ysdk = DoubleBYandexSDKBridge.ysdk;
        return DoubleBYandexSDKBridge.toPtr(ysdk && ysdk.environment ? JSON.stringify(ysdk.environment) : '');
    },

    YandexSDK_IsAvailableMethod: function (methodNamePtr, gameObjectNamePtr, callbackIdPtr) {
        var methodName = DoubleBYandexSDKBridge.toString(methodNamePtr);
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                if (!ysdk.isAvailableMethod) {
                    return { available: false };
                }

                return ysdk.isAvailableMethod(methodName).then(function (available) {
                    return { available: !!available };
                });
            }));
    },

    YandexSDK_LoadingReady: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                ysdk.features.LoadingAPI.ready();
                return { ready: true };
            }));
    },

    YandexSDK_GameplayStart: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                ysdk.features.GameplayAPI.start();
                return { started: true };
            }));
    },

    YandexSDK_GameplayStop: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                ysdk.features.GameplayAPI.stop();
                return { stopped: true };
            }));
    },

    YandexSDK_ShowFullscreenAdv: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.requireSdk()
            .then(function (ysdk) {
                ysdk.adv.showFullscreenAdv({
                    callbacks: {
                        onClose: function (wasShown) {
                            DoubleBYandexSDKBridge.send(gameObjectName, callbackId, true, JSON.stringify({ wasShown: !!wasShown }), '');
                        },
                        onError: function (error) {
                            DoubleBYandexSDKBridge.send(gameObjectName, callbackId, false, '', DoubleBYandexSDKBridge.errorToString(error));
                        }
                    }
                });
            })
            .catch(function (error) {
                DoubleBYandexSDKBridge.send(gameObjectName, callbackId, false, '', DoubleBYandexSDKBridge.errorToString(error));
            });
    },

    YandexSDK_ShowRewardedVideo: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);
        var rewarded = false;

        DoubleBYandexSDKBridge.requireSdk()
            .then(function (ysdk) {
                ysdk.adv.showRewardedVideo({
                    callbacks: {
                        onRewarded: function () {
                            rewarded = true;
                        },
                        onClose: function (wasShown) {
                            DoubleBYandexSDKBridge.send(gameObjectName, callbackId, true, JSON.stringify({
                                wasShown: !!wasShown,
                                rewarded: rewarded
                            }), '');
                        },
                        onError: function (error) {
                            DoubleBYandexSDKBridge.send(gameObjectName, callbackId, false, '', DoubleBYandexSDKBridge.errorToString(error));
                        }
                    }
                });
            })
            .catch(function (error) {
                DoubleBYandexSDKBridge.send(gameObjectName, callbackId, false, '', DoubleBYandexSDKBridge.errorToString(error));
            });
    },

    YandexSDK_GetBannerAdvStatus: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                return ysdk.adv.getBannerAdvStatus();
            }));
    },

    YandexSDK_ShowBannerAdv: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                return ysdk.adv.showBannerAdv();
            }));
    },

    YandexSDK_HideBannerAdv: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                return ysdk.adv.hideBannerAdv();
            }));
    },

    YandexSDK_GetPlayer: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePlayer().then(function (player) {
                return {
                    authorized: player.isAuthorized ? player.isAuthorized() : false,
                    id: player.getUniqueID ? player.getUniqueID() : '',
                    name: player.getName ? player.getName() : '',
                    photoSmall: player.getPhoto ? player.getPhoto('small') : '',
                    photoMedium: player.getPhoto ? player.getPhoto('medium') : '',
                    photoLarge: player.getPhoto ? player.getPhoto('large') : ''
                };
            }));
    },

    YandexSDK_OpenAuthDialog: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk()
                .then(function (ysdk) { return ysdk.auth.openAuthDialog(); })
                .then(function () {
                    DoubleBYandexSDKBridge.player = null;
                    return DoubleBYandexSDKBridge.requirePlayer();
                })
                .then(function (player) {
                    return { authorized: player.isAuthorized ? player.isAuthorized() : false };
                }));
    },

    YandexSDK_GetPlayerData: function (keysJsonPtr, gameObjectNamePtr, callbackIdPtr) {
        var keys = DoubleBYandexSDKBridge.parseJson(DoubleBYandexSDKBridge.toString(keysJsonPtr), undefined);
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePlayer().then(function (player) {
                return player.getData(keys);
            }));
    },

    YandexSDK_SetPlayerData: function (dataJsonPtr, flush, gameObjectNamePtr, callbackIdPtr) {
        var data = DoubleBYandexSDKBridge.parseJson(DoubleBYandexSDKBridge.toString(dataJsonPtr), {});
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePlayer().then(function (player) {
                return player.setData(data, !!flush).then(function () { return { saved: true }; });
            }));
    },

    YandexSDK_GetPlayerStats: function (keysJsonPtr, gameObjectNamePtr, callbackIdPtr) {
        var keys = DoubleBYandexSDKBridge.parseJson(DoubleBYandexSDKBridge.toString(keysJsonPtr), undefined);
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePlayer().then(function (player) {
                return player.getStats(keys);
            }));
    },

    YandexSDK_SetPlayerStats: function (statsJsonPtr, gameObjectNamePtr, callbackIdPtr) {
        var stats = DoubleBYandexSDKBridge.parseJson(DoubleBYandexSDKBridge.toString(statsJsonPtr), {});
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePlayer().then(function (player) {
                return player.setStats(stats).then(function () { return { saved: true }; });
            }));
    },

    YandexSDK_IncrementPlayerStats: function (statsJsonPtr, gameObjectNamePtr, callbackIdPtr) {
        var stats = DoubleBYandexSDKBridge.parseJson(DoubleBYandexSDKBridge.toString(statsJsonPtr), {});
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePlayer().then(function (player) {
                return player.incrementStats(stats);
            }));
    },

    YandexSDK_SetLeaderboardScore: function (leaderboardNamePtr, score, extraDataPtr, gameObjectNamePtr, callbackIdPtr) {
        var leaderboardName = DoubleBYandexSDKBridge.toString(leaderboardNamePtr);
        var extraData = DoubleBYandexSDKBridge.toString(extraDataPtr);
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                return ysdk.leaderboards.setScore(leaderboardName, score, extraData || undefined)
                    .then(function () { return { saved: true }; });
            }));
    },

    YandexSDK_GetLeaderboardEntries: function (leaderboardNamePtr, quantityTop, includeUser, gameObjectNamePtr, callbackIdPtr) {
        var leaderboardName = DoubleBYandexSDKBridge.toString(leaderboardNamePtr);
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);
        var options = { quantityTop: quantityTop, includeUser: !!includeUser };

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requireSdk().then(function (ysdk) {
                return ysdk.leaderboards.getLeaderboardEntries(leaderboardName, options);
            }));
    },

    YandexSDK_InitPayments: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePayments().then(function () {
                return { initialized: true };
            }));
    },

    YandexSDK_GetCatalog: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePayments().then(function (payments) {
                return payments.getCatalog();
            }));
    },

    YandexSDK_Purchase: function (productIdPtr, gameObjectNamePtr, callbackIdPtr) {
        var productId = DoubleBYandexSDKBridge.toString(productIdPtr);
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePayments().then(function (payments) {
                return payments.purchase({ id: productId });
            }));
    },

    YandexSDK_GetPurchases: function (gameObjectNamePtr, callbackIdPtr) {
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePayments().then(function (payments) {
                return payments.getPurchases();
            }));
    },

    YandexSDK_ConsumePurchase: function (purchaseTokenPtr, gameObjectNamePtr, callbackIdPtr) {
        var purchaseToken = DoubleBYandexSDKBridge.toString(purchaseTokenPtr);
        var gameObjectName = DoubleBYandexSDKBridge.toString(gameObjectNamePtr);
        var callbackId = DoubleBYandexSDKBridge.toString(callbackIdPtr);

        DoubleBYandexSDKBridge.complete(gameObjectName, callbackId,
            DoubleBYandexSDKBridge.requirePayments().then(function (payments) {
                return payments.consumePurchase(purchaseToken).then(function () {
                    return { consumed: true };
                });
            }));
    }
});
