using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

using AnimSharp.Interpolate;

namespace AnimSharp.Animate
{
    /// <summary>
    /// The handler for all events declared in the AnimatedObject class.
    /// </summary>
    /// 
    /// <param name="o">
    /// the AnimatedObject instance that triggered the event.
    /// </param>
    public delegate void AnimatedObjectHandler(AnimatedObject o);

    /// <summary>
    /// An AnimatedObject represents an object that can have its properties animated.
    /// 
    /// AnimatedObject is basically a wrapper class for a reference value that supplies
    /// convenience method to animate the object's properties.
    /// </summary>
    public class AnimatedObject : AnimatedEntity
    {
        // The list of animations to compose into a single composite animation
        // when the StartAnimation method is called.
        private List<Animation<double>> compositeAnimations;

        /// <summary>
        /// Constructs a new AnimatedObject that animates the specified
        /// object.
        /// </summary>
        /// 
        /// <param name="o">
        /// the object to be animated by this class.
        /// </param>
        public AnimatedObject(object o)
        {
            this.Object = o;
            this.compositeAnimations = new List<Animation<double>>();
        }

        /// <summary>
        /// Animates a property in the control that is of the type System.Drawing.Color
        /// from the specified color value.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the name of a property whose type is System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="oldColor">
        /// the color to animate the property from.
        /// </param>
        public void AnimatePropertyFrom(string colorPropertyName, Color oldColor)
        {
            this.AnimatePropertyFrom(colorPropertyName, oldColor, this.DefaultDuration, this.DefaultInterpolator);
        }

        /// <summary>
        /// Animates a property in the control that is of the type System.Drawing.Color
        /// from the specified color value.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the name of a property whose type is System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="oldColor">
        /// the color to animate the property from.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function that should be used to generate intermediate
        /// color values during the animation.
        /// </param>
        public void AnimatePropertyFrom(string colorPropertyName, Color oldColor, Interpolator interpolator)
        {
            this.AnimatePropertyFrom(colorPropertyName, oldColor, this.DefaultDuration, interpolator);
        }

        /// <summary>
        /// Animates a property in the control that is of the type System.Drawing.Color
        /// from the specified color value.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the name of a property whose type is System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="oldColor">
        /// the color to animate the property from.
        /// </param>
        /// 
        /// <param name="duration">
        /// the amount of time in millseconds that the animation should last.
        /// </param>
        public void AnimatePropertyFrom(string colorPropertyName, Color oldColor, long duration)
        {
            this.AnimatePropertyFrom(colorPropertyName, oldColor, duration, this.DefaultInterpolator);
        }

        /// <summary>
        /// Animates a property in the control that is of the type System.Drawing.Color
        /// from the specified color value.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the name of a property whose type is System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="oldColor">
        /// the color to animate the property from.
        /// </param>
        /// 
        /// <param name="duration">
        /// the amount of time in millseconds that the animation should last.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function that should be used to generate intermediate
        /// color values during the animation.
        /// </param>
        public void AnimatePropertyFrom(string colorPropertyName, Color oldColor,
                                        long duration, Interpolator interpolator)
        {
            // Retrieve the specified property.
            var colorProperty = this.VerifyColorPropertyName(colorPropertyName);

            // Retrieve the current value of the color property.
            var currentColor = (Color)colorProperty.GetValue(this.Object);

            // Start a color animation that will update the color property every increment.
            var colorAnimation = new ColorAnimation(oldColor, currentColor, duration, interpolator);
            colorAnimation.AnimationIncremented += (_, __) =>
            {
                var incrementColor = colorAnimation.CurrentColor;
                this.SetPropertyValue(colorProperty, incrementColor);
            };
            this.AddAnimation(colorAnimation);

            // Start Animation
            colorAnimation.Start();
        }

        /// <summary>
        /// Animates a property in the control that is of the type System.Drawing.Color
        /// to the specified color value.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the name of a property whose type is System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="newColor">
        /// the color to animate the property to.
        /// </param>
        public void AnimatePropertyTo(string colorPropertyName, Color newColor)
        {
            this.AnimatePropertyTo(colorPropertyName, newColor, this.DefaultDuration, this.DefaultInterpolator);
        }

        /// <summary>
        /// Animates a property in the control that is of the type System.Drawing.Color
        /// to the specified color value.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the name of a property whose type is System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="newColor">
        /// the color to animate the property to.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function that should be used to generate intermediate
        /// color values during the animation.
        /// </param>
        public void AnimatePropertyTo(string colorPropertyName, Color newColor, Interpolator interpolator)
        {
            this.AnimatePropertyTo(colorPropertyName, newColor, this.DefaultDuration, interpolator);
        }

        /// <summary>
        /// Animates a property in the control that is of the type System.Drawing.Color
        /// to the specified color value.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the name of a property whose type is System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="newColor">
        /// the color to animate the property to.
        /// </param>
        /// 
        /// <param name="duration">
        /// the amount of time in millseconds that the animation should last.
        /// </param>
        public void AnimatePropertyTo(string colorPropertyName, Color newColor, long duration)
        {
            this.AnimatePropertyTo(colorPropertyName, newColor, duration, this.DefaultInterpolator);
        }

        /// <summary>
        /// Animates a property in the control that is of the type System.Drawing.Color
        /// to the specified color value.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the name of a property whose type is System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="newColor">
        /// the color to animate the property to.
        /// </param>
        /// 
        /// <param name="duration">
        /// the amount of time in millseconds that the animation should last.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function that should be used to generate intermediate
        /// color values during the animation.
        /// </param>
        public void AnimatePropertyTo(string colorPropertyName, Color newColor,
                                      long duration, Interpolator interpolator)
        {
            // Retrieve the specified property.
            var colorProperty = this.VerifyColorPropertyName(colorPropertyName);

            // Retrieve the current value of the color property.
            var currentColor = (Color)colorProperty.GetValue(this.Object);

            // Start a color animation that will update the color property every increment.
            var colorAnimation = new ColorAnimation(currentColor, newColor, duration, interpolator);
            colorAnimation.AnimationIncremented += (_, __) =>
            {
                var incrementColor = colorAnimation.CurrentColor;
                this.SetPropertyValue(colorProperty, incrementColor);
            };
            this.AddAnimation(colorAnimation);

            // Start Animation
            colorAnimation.Start();
        }

        /// <summary>
        /// Animates the object's property with the specified name to its current value
        /// from the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to animate. The property must be a public property
        /// of a primitive numeric type and be both readable and writable.
        /// </param>
        /// <param name="value">
        /// the value to animate the property from.
        /// </param>
        public void AnimatePropertyFrom(string propertyName, double value)
        {
            // Use the default interpolator and duration.
            this.AnimatePropertyFrom(propertyName, value, DefaultDuration, DefaultInterpolator);
        }

        /// <summary>
        /// Animates the object's property with the specified name to its current value
        /// from the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to animate. The property must be a public property
        /// of a primitive numeric type and be both readable and writable.
        /// </param>
        /// <param name="value">
        /// the value to animate the property from.
        /// </param>
        /// <param name="duration">
        /// the duration the animation should last in milliseconds.
        /// </param>
        public void AnimatePropertyFrom(string propertyName, double value, long duration)
        {
            // Use the default interpolator.
            this.AnimatePropertyFrom(propertyName, value, duration, DefaultInterpolator);
        }

        /// <summary>
        /// Animates the object's property with the specified name to its current value
        /// from the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to animate. The property must be a public property
        /// of a primitive numeric type and be both readable and writable.
        /// </param>
        /// <param name="value">
        /// the value to animate the property from.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function the animation should use.
        /// </param>
        public void AnimatePropertyFrom(string propertyName, double value, Interpolator interpolator)
        {
            // Use the default duration.
            this.AnimatePropertyFrom(propertyName, value, DefaultDuration, interpolator);
        }

        /// <summary>
        /// Animates the object's property with the specified name to its current value
        /// from the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to animate. The property must be a public property
        /// of a primitive numeric type and be both readable and writable.
        /// </param>
        /// <param name="value">
        /// the value to animate the property from.
        /// </param>
        /// <param name="duration">
        /// the duration the animation should last in milliseconds.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function the animation should use.
        /// </param>
        public void AnimatePropertyFrom(string propertyName, double value, long duration,
                                        Interpolator interpolator)
        {
            // Verify the property name.
            var property = this.VerifyPropertyName(propertyName);

            // Retrieve the current value of the property.
            var propertyValue = this.GetPropertyValue(property);
            var endValue = (double)Convert.ChangeType(propertyValue, typeof(double));

            // Create an animation animating from the property value to the specified
            // value.
            var animation = new ValueAnimation(value, endValue, duration, interpolator);
            this.AddAnimation(animation);

            // Set the value of the property on every increment of the animation.
            animation.AnimationIncremented += (_, __) =>
            {
                var currentValue = Convert.ChangeType(animation.CurrentValue, property.PropertyType);
                this.SetPropertyValue(property, currentValue);
            };

            // Start the animation.
            animation.Start();
        }

        /// <summary>
        /// Animates the object's property with the specified name to the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to animate. The property must be a public property
        /// of a primitive numeric type and be both readable and writable.
        /// </param>
        /// <param name="value">
        /// the value to animate the property to.
        /// </param>
        public void AnimatePropertyTo(string propertyName, double value)
        {
            // Use the default duration and interpolator.
            this.AnimatePropertyTo(propertyName, value, DefaultDuration, DefaultInterpolator);
        }

        /// <summary>
        /// Animates the object's property with the specified name to the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to animate. The property must be a public property
        /// of a primitive numeric type and be both readable and writable.
        /// </param>
        /// <param name="value">
        /// the value to animate the property to.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function the animation should use.
        /// </param>
        public void AnimatePropertyTo(string propertyName, double value, Interpolator interpolator)
        {
            // Use the default duration.
            this.AnimatePropertyTo(propertyName, value, DefaultDuration, interpolator);
        }

        /// <summary>
        /// Animates the object's property with the specified name to the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to animate. The property must be a public property
        /// of a primitive numeric type and be both readable and writable.
        /// </param>
        /// <param name="value">
        /// the value to animate the property to.
        /// </param>
        /// <param name="duration">
        /// the duration the animation should last in milliseconds.
        /// </param>
        public void AnimatePropertyTo(string propertyName, double value, long duration)
        {
            // Use the default interpolator.
            this.AnimatePropertyTo(propertyName, value, duration, DefaultInterpolator);
        }

        /// <summary>
        /// Animates the object's property with the specified name to the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to animate. The property must be a public property
        /// of a primitive numeric type and be both readable and writable.
        /// </param>
        /// <param name="value">
        /// the value to animate the property to.
        /// </param>
        /// <param name="duration">
        /// the duration the animation should last in milliseconds.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function the animation should use.
        /// </param>
        public void AnimatePropertyTo(string propertyName, double value, long duration,
                                      Interpolator interpolator)
        {
            var property = VerifyPropertyName(propertyName);

            // Retrieve the current value of the property.
            var propertyValue = this.GetPropertyValue(property);
            var startValue = (double)Convert.ChangeType(propertyValue, typeof(double));

            // Create an animation animating from the property value to the specified
            // value.
            var animation = new ValueAnimation(startValue, value, duration, interpolator);
            this.AddAnimation(animation);

            // Set the value of the property on every increment of the animation.
            animation.AnimationIncremented += (_, __) =>
            {
                var currentValue = Convert.ChangeType(animation.CurrentValue, property.PropertyType);
                SetPropertyValue(property, currentValue);

                if (ObjectPropertyChanged != null)
                    ObjectPropertyChanged(this);
            };

            // Start the animation.
            animation.Start();
        }

        /// <summary>
        /// Animates the values of all of the object's primitive properties
        /// to the values of all of the properties with the same names
        /// and types in the specified object.
        /// </summary>
        /// 
        /// <param name="target">
        /// the object who contains similarly named properties as the object
        /// whose values should be animated to.
        /// </param>
        public void AnimateTo(object target)
        {
            // If the object is immutable, it cannot be animated.
            if (IsImmutable())
                throw new InvalidOperationException("An immutable object cannot be animated");

            // The value being animated to needs to be the same type as the wrapped
            // object.
            var type = Object.GetType();
            var targetType = target.GetType();
            if (!type.IsAssignableFrom(targetType))
                throw new ArgumentException("The value being animated to must be the same type as the wrapped object");

            // Only primitive instance properties should be animated.
            var searchFlags = BindingFlags.Instance;
            var objectProperties = from property in type.GetProperties(searchFlags)
                                   where property.PropertyType.IsPrimitive
                                   select property;

            var lastProperty = objectProperties.Last();
            foreach (var property in objectProperties)
            {
                var startValue = (double)property.GetValue(Object);
                var endValue = (double)property.GetValue(target);
                var propertyAnimation = new ValueAnimation(startValue, endValue, DefaultDuration, DefaultInterpolator);
                propertyAnimation.AnimationIncremented += (_, __) => property.SetValue(Object, propertyAnimation.CurrentValue);
                this.AddAnimation(propertyAnimation);

                // Once the last property is incremented, the ObjectChanged event should be triggered.
                if (property.Equals(lastProperty))
                    propertyAnimation.AnimationIncremented += (_, __) =>
                    {
                        if (ObjectChanged != null)
                            ObjectChanged(this);
                    };

                propertyAnimation.Start();
            }
        }

        /// <summary>
        /// Begins composing animations performed on the AnimatedObject.
        /// 
        /// When animations are being composed, all animations performed
        /// on the object will be saved instead of started until the
        /// StartAnimation method is called. Once the StartAnimation
        /// method is called, all saved animations will be started as
        /// a single CompositeAnimation.
        /// 
        /// Any subsequent calls to the ComposeAnimation method before
        /// the StartAnimation method is called will result in the
        /// saved animations being cleared.
        /// </summary>
        public void ComposeAnimation()
        {
            this.Composing = true;
            this.compositeAnimations.Clear();
        }

        /// <summary>
        /// Gets whether or not animations are currently being composed
        /// into a single composite animation. This will only be true
        /// in between calls to the ComposeAnimation
        /// method and the StartAnimation method.
        /// </summary>
        public bool Composing { get; private set; }

        /// <summary>
        /// Gets the value of the specified property for the wrapped object.
        /// </summary>
        /// 
        /// <param name="property">
        /// the property whose value should be retrieved for the wrapped object.
        /// </param>
        /// 
        /// <returns>
        /// the property value retrieved as an object.
        /// </returns>
        protected virtual object GetPropertyValue(PropertyInfo property)
        {
            return property.GetValue(this.Object);
        }

        /// <summary>
        /// Determines whether the wrapped object is immutable.
        /// 
        /// An Object is considered immutable if all of its primitive properties
        /// are read only properties.
        /// </summary>
        /// 
        /// <returns>
        /// true if the object is considered immutable, or false if it isn't.
        /// </returns>
        public bool IsImmutable()
        {
            // To determine if the object is immutable, the wrapped object's
            // properties must be searched to see if any are writeable. 
            var type = Object.GetType();
            var propertySearchFlags = BindingFlags.Public | BindingFlags.Instance;
            var properties = type.GetProperties(propertySearchFlags);
            var primitiveProperties = from property in properties
                                      where property.PropertyType.IsPrimitive
                                            && property.CanWrite
                                            && !property.SetMethod.IsPrivate
                                      select property;

            // If at least one property is found that is not read only,
            // the object is not considered immutable.
            return primitiveProperties.Count() == 0;
        }

        /// <summary>
        /// The object whose properties are animated by this class.
        /// </summary>
        public object Object { get; private set; }

        /// <summary>
        /// Sets the value of the property of the specified name for the wrapped
        /// Object instance.
        /// 
        /// The property must be a primitive typed property.
        /// </summary>
        /// 
        /// <param name="property">
        /// the name of the property to set the value for.
        /// </param>
        /// <param name="value">
        /// the value to set the property's valeu to.
        /// </param>
        protected virtual void SetPropertyValue(PropertyInfo property, object value)
        {
            // Set the value of the property for the wrapped object.
            property.SetValue(this.Object, value);
        }

        /// <summary>
        /// Creates a CompositeAnimation out of all of the animations that were
        /// saved since the ComposeAnimation method was called and starts it.
        /// 
        /// Once this method is called, the AnimatedObject will no longer be
        /// composing animations until the ComposeAnimation method is called
        /// again. 
        /// 
        /// If this method is called before the ComposeAnimation method is called
        /// or when no animations have been saved, nothing will happen.
        /// </summary>
        public void StartAnimation(long duration, Interpolator interpolator)
        {
            // Stop composing.
            this.Composing = false;

            // Do nothing if there are no saved animations.
            if (this.compositeAnimations.Count == 0)
                return;

            // Create composite animation.
            var animation = new CompositeAnimation<double>(this.compositeAnimations.ToArray());
            animation.Start();
        }

        /// <summary>
        /// Verifies that the contained control contains a property of the specified
        /// name that is readable and writable and is of the type System.Drawing.Color.
        /// </summary>
        /// 
        /// <param name="colorPropertyName">
        /// the property name to verify.
        /// </param>
        /// 
        /// <returns>
        /// the PropertyInfo for the found property if and only if the property
        /// name was valid.
        /// </returns>
        protected PropertyInfo VerifyColorPropertyName(string colorPropertyName)
        {
            // Ensure the property name is not empty.
            if (string.IsNullOrWhiteSpace(colorPropertyName))
                throw new ArgumentException(
                    "The property name cannot be null, empty, or whitespace."
                );

            PropertyInfo propertyInfo;
            try
            {
                propertyInfo = this.Object.GetType().GetProperty(colorPropertyName);

                // Ensure the property was found and is for a color.
                if (propertyInfo == null)
                    throw new ArgumentException(string.Format(
                        "The property \"{0}\" was not found.",
                        colorPropertyName
                    ));

                if (!typeof(Color).IsAssignableFrom(propertyInfo.PropertyType))
                    throw new ArgumentException(string.Format(
                        "The property \"{0}\" was not of type System.Drawing.Color",
                        colorPropertyName
                    ));

                // Ensure the property is writeable.
                if (!propertyInfo.CanWrite)
                    throw new ArgumentException(string.Format(
                        "The property \"{0}\" is not writeable",
                        colorPropertyName
                    ));
            }

            // If multiple matches of the property were found, don't proceed.
            catch (AmbiguousMatchException e)
            {
                throw e;
            }

            return propertyInfo;
        }

        /// <summary>
        /// Verifies that the contained object contains a property of the specified
        /// name that is readable and writable and is of a primitive type.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the property name to verify.
        /// </param>
        /// 
        /// <returns>
        /// the PropertyInfo for the found property if and only if the property
        /// name was valid.
        /// </returns>
        protected PropertyInfo VerifyPropertyName(string propertyName)
        {
            // Ensure a valid property name was given.
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(
                    "The property name cannot be null, empty, or whitespace"
                );

            // Ensure the object contains a property with the specified property name
            // that is a primitive type and is both readable and writeable.
            PropertyInfo propertyInfo;
            try
            {
                // Attempt to retrieve the property info for the given property name.
                propertyInfo = Object.GetType().GetProperty(propertyName);

                // Ensure a property was found.
                if (propertyInfo == null)
                    throw new ArgumentException(
                        "The property \"" + propertyName + "\" was not found"
                    );

                // Ensure the property is a primitive type.
                if (!propertyInfo.PropertyType.IsPrimitive)
                    throw new ArgumentException(
                        "The property \"" + propertyName + "\" is not a primitive type"
                    );

                // Ensure the property is writable.
                if (!propertyInfo.CanWrite)
                    throw new ArgumentException(
                        "The property \"" + propertyName + "\" is not writable."
                    );
            }

            // If multiple matches for the property were found, don't proceed.
            catch (AmbiguousMatchException e)
            {
                throw e;
            }

            // If all criteria was met, return the found property info.
            return propertyInfo;
        }

        /// <summary>
        /// Called after each animation frame when the object is animated.
        /// </summary>
        public event AnimatedObjectHandler ObjectChanged;

        /// <summary>
        /// Called after each animation frame when an object property is animated.
        /// </summary>
        public event AnimatedObjectHandler ObjectPropertyChanged;
    }
}
