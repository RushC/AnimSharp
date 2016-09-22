using System.Collections.Generic;
using System.Linq;

using AnimSharp.Interpolate;

namespace AnimSharp.Animate
{
    /// <summary>
    /// An AnimatedCollection represents a collection that can have
    /// iteration logic animated.
    /// 
    /// AnimatedCollection is basically a wrapper class for a collection that
    /// supplies convenience methods to animate through the collection's
    /// values.
    /// 
    /// Note that this class wraps a read-only copy of the collection
    /// at the time that the AnimatedCollection instance is created.
    /// This means that the elements that can be iterated through
    /// an AnimatedCollection instance cannot be changed once after
    /// the AnimatedCollection is created.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// the type of the elements stored in the collection.
    /// </typeparam>
    public class AnimatedCollection<T> : AnimatedEntity
    {
        /// <summary>
        /// Constructs a new AnimatedCollection that wraps the
        /// specified collection.
        /// </summary>
        /// 
        /// <param name="collection">
        /// the collection to be wrapped by the AnimatedCollection
        /// instance.
        /// </param>
        public AnimatedCollection(IEnumerable<T> collection)
        {
            // Save a read-only copy of the collection.
            this.Collection = Enumerable.ToList(collection).AsReadOnly();
        }

        /// <summary>
        /// Gets the collection that is weapped by this AnimatedCollection
        /// instance.
        /// </summary>
        public IEnumerable<T> Collection { get; private set; }

        /// <summary>
        /// Performs an animated iteration through the wrapped collection's
        /// elements.
        /// </summary>
        public void ForEach(IterationAction action)
        {
            this.ForEach(action, this.DefaultDuration, this.DefaultInterpolator);
        }

        /// <summary>
        /// Performs an animated iteration through the wrapped collection's
        /// elements.
        /// </summary>
        /// 
        /// <param name="action">
        /// the action to perform for every iteration through the collection.
        /// </param>
        /// <param name="duration">
        /// the number of milliseconds that the animation should last.
        /// </param>
        public void ForEach(IterationAction action, long duration)
        {
            this.ForEach(action, duration, this.DefaultInterpolator);
        }

        /// <summary>
        /// Performs an animated iteration through the wrapped collection's
        /// elements.
        /// </summary>
        /// 
        /// <param name="action">
        /// the action to perform for every iteration through the collection.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function that should be used for the animation.
        /// </param>
        public void ForEach(IterationAction action, Interpolator interpolator)
        {
            this.ForEach(action, this.DefaultDuration, interpolator);
        }

        /// <summary>
        /// Performs an animated iteration through the wrapped collection's
        /// elements.
        /// </summary>
        /// 
        /// <param name="action">
        /// the action to perform for every iteration through the collection.
        /// </param>
        /// <param name="duration">
        /// the number of milliseconds that the animation should last.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function that should be used for the animation.
        /// </param>
        public void ForEach(IterationAction action, long duration, 
                            Interpolator interpolator)
        {
            // Perform an animation that will animate through
            // all of the indices in the collection.
            var lastIndex = this.Collection.Count() - 1;
            var indexAnimation = new ValueAnimation(0, lastIndex, 
                                                    duration, interpolator);

            // Every time the index animation is iterated, iterate the
            // "loop" and perform the action for every item up to the
            // current index.
            int i = 0;
            indexAnimation.AnimationIncremented += (_, __) =>
            {
                while (i <= indexAnimation.CurrentValue)
                {
                    var element = this.Collection.ElementAt(i);
                    action(element, i);
                    i++;
                }
            };

            // Add the index animation to the list of animations being
            // performed.
            this.AddAnimation(indexAnimation);

            // Start the animation.
            indexAnimation.Start();
        }

        /// <summary>
        /// The type of a method performed in the iteration of an
        /// AnimatedCollection.
        /// </summary>
        /// 
        /// <param name="element">
        /// the element in the collection currently being iterated over.
        /// </param>
        /// 
        /// <param name="index">
        /// the index of the item.
        /// </param>
        public delegate void IterationAction(T element, int index);
    }
}
