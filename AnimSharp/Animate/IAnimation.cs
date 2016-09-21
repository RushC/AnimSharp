using System.ComponentModel;

namespace AnimSharp.Animate
{
    /// <summary>
    /// The delegate type of handlers for all IAnimation events.
    /// </summary>
    /// 
    /// <param name="source">
    /// the generic IAnimation object that triggered the event.
    /// </param>
    public delegate void AnimationEventHandler(IAnimation source, AnimationEventArgs e);

    /// <summary>
    /// The delegate type of methods to be called asynchronously after an animation ends.
    /// </summary>
    public delegate void PostAnimationAction();

    /// <summary>
    /// Classes that implement the IAnimation interface represent values that can
    /// be gradually changed over time.
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// Advances the animation the specified amount of time.
        /// </summary>
        /// 
        /// <param name="milliseconds">
        /// the number of milliseconds to advance the animation.
        /// </param>
        void Advance(long milliseconds);

        /// <summary>
        /// Called when the Animation has finished.
        /// </summary>
        event AnimationEventHandler AnimationEnded;

        /// <summary>
        /// Called when the Animation is started.
        /// </summary>
        event AnimationEventHandler AnimationStarted;

        /// <summary>
        /// Called if an Animation is stopped via the
        /// Stop method.
        /// </summary>
        event AnimationEventHandler AnimationStopped;

        /// <summary>
        /// Called before the animation is incremented.
        /// </summary>
        event AnimationEventHandler AnimationPreIncrement;

        /// <summary>
        /// Called every time the animation is incremented.
        /// </summary>
        event AnimationEventHandler AnimationIncremented;

        /// <summary>
        /// Called after the animation is incremented.
        /// </summary>
        event AnimationEventHandler AnimationPostIncrement;

        /// <summary>
        /// Blocks the current thread until the animation
        /// is completed.
        /// </summary>
        /// 
        /// <remarks>
        /// An animation is only considered completed when
        /// the Ended property becomes true. If an Animation
        /// is stopped or for some reason prevented from
        /// advancing, it is possible this method will never
        /// return. Use this method with caution.
        /// 
        /// One should also be wary about calling this method
        /// during events. If the thread handling the event
        /// created a control, that control will be unable
        /// to update itself, since the thread that created
        /// it is the only thread allowed to modify it and is
        /// blocked. This can be counterproductive if the
        /// animation is intended to animate the control,
        /// as it will simply result in the control freezing
        /// for the duration of the animation.
        /// </remarks>
        void Await();

        /// <summary>
        /// Blocks the current thread until the animation
        /// is completed and then sleeps for the specified
        /// amount of time.
        /// </summary>
        /// 
        /// <param name="milliseconds">
        /// the amount of time in milliseconds to sleep for
        /// after the animation finishes.
        /// </param>
        /// 
        /// <exception cref="System.ArgumentException">
        /// if the milliseconds value is less than zero.
        /// </exception>
        /// 
        /// <remarks>
        /// An animation is only considered completed when
        /// the Ended property becomes true. If an Animation
        /// is stopped or for some reason prevented from
        /// advancing, it is possible this method will never
        /// return. Use this method with caution.
        /// 
        /// One should also be wary about calling this method
        /// during events. If the thread handling the event
        /// created a control, that control will be unable
        /// to update itself, since the thread that created
        /// it is the only thread allowed to modify it and is
        /// blocked. This can be counterproductive if the
        /// animation is intended to animate the control,
        /// as it will simply result in the control freezing
        /// for the duration of the animation.
        /// </remarks>
        void Await(int milliseconds);

        /// <summary>
        /// Gets the duration of the animation in milliseconds.
        /// </summary>
        long Duration { get; }

        /// <summary>
        /// Gets the amount of time that has elapsed in the animation
        /// in milliseconds.
        /// </summary>
        long ElapsedTime { get; }

        /// <summary>
        /// Gets whether or not the animation has completed.
        /// </summary>
        bool Ended { get; }

        /// <summary>
        /// Immediately finishes the animation.
        /// </summary>
        /// 
        /// <remarks>
        /// The Finish method is the ideal alternative to the Stop
        /// method when you wish for the animation to still complete
        /// and trigger its AnimationEnd event. Note that this method
        /// will perform an increment in order to finish the animation,
        /// meaning that the AnimationIncrement events will still be
        /// called one more time.
        /// </remarks>
        void Finish();

        /// <summary>
        /// Resets the animation to its initial state.
        /// </summary>
        void Reset();

        /// <summary>
        /// Starts animating from the start value to the end value
        /// using the Animator class.
        /// </summary>
        void Start();

        /// <summary>
        /// Gets whether or not the animation has started.
        /// </summary>
        bool Started { get; }

        /// <summary>
        /// Stops the animation at its current state.
        /// </summary>
        /// 
        /// <remarks>
        /// Be careful when stopping animations. Stopping an animation
        /// will prevent the animation from finishing unless the animation
        /// is later resumed using the Start method. This means that the
        /// animation won't call trigger the AnimationEnd event, since the
        /// animation never finished. This is especially dangerous if a
        /// thread has called the Await method on the animation, as that
        /// thread will be stuck waiting for an animation that may not
        /// complete.
        /// </remarks>
        void Stop();

        /// <summary>
        /// Gets or sets the object to synchronize with. Setting this
        /// object will force all events fired by the animation to be
        /// fired on the object's synchronized thread.
        /// </summary>
        ISynchronizeInvoke SynchronizationObject { get; set; }

        /// <summary>
        /// Performs the specified action after the animation finishes.
        /// </summary>
        /// 
        /// <param name="action">
        /// the action to perform after the animation finishes.
        /// </param>
        /// 
        /// <remarks>
        /// This is an alternative to the Await method. The difference
        /// between this method and Await is that this method will not
        /// block the current thread, and will instead perform the 
        /// specified action asynchronously when the animation finishes.
        /// 
        /// If the animation is already completed when this method is
        /// called, the action will be started immediately, though still
        /// asynchronously.
        /// </remarks>
        void Then(PostAnimationAction action);
    }
}
