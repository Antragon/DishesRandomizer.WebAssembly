namespace MealsRandomizer.WebAssembly.Models;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Timers;

public class Dice {
    private readonly Subject<int> _onRotationChanged = new();

    private int _rotation;

    public IObservable<int> OnRotationChanged => _onRotationChanged.AsObservable();

    public bool Clickable { get; private set; } = true;

    public int Rotation {
        get => _rotation;
        private set => SetRotation(value);
    }

    private void SetRotation(int value) {
        _rotation = value;
        _onRotationChanged.OnNext(value);
    }

    public async void Shuffle() {
        try {
            Clickable = false;
            using var timer = new Timer(300);
            var running = true;
            timer.Elapsed += (_, _) => running = false;
            timer.Start();
            while (running) {
                Rotation = new Random().Next(0, 361);
                await Task.Delay(20);
            }
        } finally {
            Clickable = true;
            Rotation = 0;
        }
    }
}