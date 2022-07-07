﻿namespace DishesRandomizer.WebAssembly;

using Blazored.LocalStorage;
using Common;

public class CookbookProvider {
    private const string _storageKey = "cookbook";

    private readonly ILocalStorageService _localStorageService;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private CookbookController? _cookbookController;

    public CookbookProvider(ILocalStorageService localStorageService) {
        _localStorageService = localStorageService;
    }

    public async Task<CookbookController> Get() {
        if (_cookbookController == null) {
            await InitializeCookbookController();
        }

        return _cookbookController!;
    }

    private async Task InitializeCookbookController() {
        var storedCookbook = await _localStorageService.GetItemAsync<Cookbook?>(_storageKey);
        var cookbook = storedCookbook ?? Cookbook.Default;
        _cookbookController = new CookbookController(cookbook);
        _cookbookController.CookbookChanged.Subscribe(Save);
    }

    private async void Save(Cookbook cookbook) {
        await _semaphore.WaitAsync();
        try {
            await _localStorageService.SetItemAsync(_storageKey, cookbook);
        } finally {
            _semaphore.Release();
        }
    }
}