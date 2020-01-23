using System;
using System.Runtime.CompilerServices;
using System.Threading;
using DG.Tweening;


namespace Fujiwara {

    /// <summary>
    /// DoTween async/await対応
    /// 参考：https://qiita.com/tatsunoru/items/095a29ff375a4ddc6806
    /// </summary>
    public static class DOTweenExtention
    {
        // TweenのAwaiter
        public struct TweenAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion
        {
            Tween tween;

            public TweenAwaiter(Tween tween) => this.tween = tween;

            // 最初にすでに終わってるのか終わってないのかの判定のために呼び出されるメソッドらしい
            public bool IsCompleted => tween.IsComplete();

            // Tweenは値を返さないので特に処理がいらないと思う
            public void GetResult() { }

            // このAwaiterの処理が終わったらcontinuationを呼び出してほしいって感じのメソッドらしい
            public void OnCompleted(System.Action continuation) => tween.OnKill(() => continuation());

            // OnCompletedと同じでいいっぽい？
            public void UnsafeOnCompleted(System.Action continuation) => tween.OnKill(() => continuation());
        }

        // Tweenに対する拡張メソッド
        public static TweenAwaiter GetAwaiter(this Tween self)
        {
            return new TweenAwaiter(self);
        }
    }


    public struct TweenAwaiter : ICriticalNotifyCompletion
    {
        Tween tween;
        CancellationToken cancellationToken;

        public TweenAwaiter(Tween tween, CancellationToken cancellationToken)
        {
            this.tween = tween;
            this.cancellationToken = cancellationToken;
        }

        public bool IsCompleted => !tween.IsPlaying();

        public void GetResult() => cancellationToken.ThrowIfCancellationRequested();

        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);

        public void UnsafeOnCompleted(Action continuation)
        {
            CancellationTokenRegistration regist = new CancellationTokenRegistration();
            var tween = this.tween;

            // Tweenが死んだら続きを実行
            tween.OnKill(() =>
            {
                regist.Dispose(); // CancellationTokenRegistrationを破棄する
                continuation(); // 続きを実行
            });

            // tokenが発火したらTweenをKillする
            regist = cancellationToken.Register(() =>
            {
                tween.Kill(true);
            });
        }

        public TweenAwaiter GetAwaiter() => this;
    }

    public static class TweenAwaiterExtention
    {
        // TweenにToAwaiter拡張メソッドを追加
        public static TweenAwaiter ToAwaiter(this Tween self, CancellationToken cancellationToken = default)
        {
            return new TweenAwaiter(self, cancellationToken);
        }
    }
}
