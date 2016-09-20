# AnimSharp
AnimSharp is an animation library that allows you to animate values, objects, and even WinForms Controls easily.

_______________________________________________________________________________________________________________

### Animating a Simple Value
To perform a simple animation for a double value, you can create a new **ValueAnimation** and start it.

First, use the Animate namespace.
```C#
using AnimSharp.Animate;
```

Then declare and start a new **ValueAnimation**.

```C#
var animation = new ValueAnimation(0, 50);
animation.Start();
```

That's it! That will animate a value from 0 to 50. Of course, it's pretty useless since it doesn't actually do
anything to show us that it's animating. Let's change that.

```C#
var animation = new ValueAnimatio(0, 50);

// Print the animated value to the console on every animation frame.
animation.AnimationIncremented += (source, args) =>
{
  Console.WriteLine(animation.CurrentValue.ToString());
};

animation.Start()
```

There! We registered an event handler on the ValueAnimation's AnimationIncremented event to print the value that the animation
is at every time its value is incremented. Now the output should look something like this:

```
2.25
4.25
6.25
8.25
10.25
12.25
14.375
16.5
18.5
20.5
22.5
24.5
26.625
28.625
30.625
32.625
34.625
36.625
38.625
40.625
42.625
44.75
46.75
48.875
50
```

At the moment, the animation lasts some unspecified amount of time. You can specify the duration of the animation in the
constructor.

```C#
var animation = new ValueAnimation(0, 50, 780);   // Now the animation will last for 780 milliseconds.
// ...
```

Aside from the duration, you can also change the "Easing Function" of the animation (referred to as Interpolators inside
the API). The Easing Function of the animation determines how fast the value moves at specific time frames. By default,
the Linear Easing Function is used for all animations. This means that our value currently moves at a constant speed
throughout the entire animation. That's pretty boring. If we use the Accelerating easing function, the value will start
out moving slowly and quickly accelerate towards the end of the animation.

To specify the easing function, you'll first need to include the Interpolate namespace:

```C#
using AnimSharp.Interpolate;
```

Once again, you can use the constructor of the animation to change it.

```C#
var animation = new ValueAnimation(0, 50, Interpolators.Accelerating);
// ...
```

Now the output will look more like this:

```
0.0903125
0.3403125
0.7503125
1.3203125
2.0503125
3.00125
4.06125
5.28125
6.66125
8.20125
9.90125
11.76125
13.78125
15.96125
18.4528125
20.9628125
23.6328125
26.4628125
29.645
32.805
36.125
39.605
43.245
47.045
50
```

Notice that the first half of the values are much closer together compared to the original animation while the second half are
much farther apart. 

You can also specify your own custom easing function. A valid Easing Function should take in a single double input that represents
the percent of time that passed (0.00 to 1.00) and return the percent of progress the value should make at that time (0.00 to 1.00).
An example of a valid Easing Function would be the Sqrt function.

```C#
var animation = new ValueAnimation(0, 50, Math.Sqrt);
// ...
```

The square root function works well because it will return 0.0 when the input is 0.0, meaning that when no time has passed, no
progress will be made. Likewise, when the input is 1.0, it will return 1.0, meaning that when the entire time has passed, the
input will be completely progressed to its final position.

Of course, you can give a function that returns pretty much anything. If your function returns a crazy value like 976 at any
point, though, the animation probably isn't going to look so smooth.

_______________________________________________________________________________________________________________

### Animating an Object's Property

While the ValueAnimation technically contains enough logic to perform any animation you want, it would be tedious to have to
attach an event handler every time you wanted to animate a value. In most cases, you would probably just be animating an Object's
property anyways. That's where the **AnimatedObject** class comes in.

Let's define a simple class:

```C#
class Point
{
  public double X { get; set; }
  public double Y { get; set; }
}
```

This class represents a simple Point that starts at (0, 0). Say we want to animate that point to (50, 0). While we could use
a ValueAnimation and attach an event handler that will set the Point's X property, it will be a lot easier to let the
AnimatedObject class handle that for us.

First, make sure the Animate namespace is included.

```C#
using AnimSharp.Animate;
```

Now, let's create the point we want to animate.

```C#
var point = new Point();
```

And now, let's wrap that point inside an AnimatedObject.

```C#
var animatedPoint = new AnimatedObject(point);
```

AnimatedObject is just a wrapper class. Once you wrap an object in an AnimatedObject instance, any operation you perform on the
AnimatedObject will affect the wrapped object.

We can animate the X property like this:

```C#
animatedPoint.AnimatePropertyTo("X", 50);
```

And that's it! The AnimatedObject class uses reflection to automatically set property values on every increment when you use the
AnimatePropertyTo method. Of course, if you try to animate a property that doesn't exist, an exception will be raised.

The AnimatePropertyTo method currently only works for primitive numeric types and Color values. If you try to animate a property
that isn't one of those types, an exception will be raised.

As you can probably guess from the name of the AnimatePropertyTo method, there is also an AnimatePropertyFrom method, which simply
animates backwards from whatever value you specify to its current value.

There are also overloads for both methods that accept durations and easing functions as well. However, if you have one set of options
that you really like, you can set the default duration and easing function for an AnimatedObject instance that will get used for any
animation where they aren't specified.

```C#
animatedPoint.DefaultDuration = 780;
animatedPoint.DefaultInterpolator = Interpolators.Accelerating;

animatedPoint.AnimatePropertyTo("X", 100);                            // Will use the Accelerating easing function and last 780 milliseconds.
animatedPoint.AnimatePropertyTo("Y", 100, 250, Interpolators.Linear); // Will use the Linear easing function and last 250 milliseconds.
```

Also, if you still have something you wish to do whenever the property is incremented, you can still attach an event handler to the
AnimatedObject's ObjectPropertyChanged event.

_______________________________________________________________________________________________________________

### Animating a Control

So if you're looking for an animation library, you're more than likely looking to animate a WinForms control. In this case,
you would actually be out of luck with the AnimatedObject class, or, at least, you'd run into quite a few issues. Animations
inherently need multiple threads (unless you want an animation to completely block the thread you're on, but that's not
very productive). Unfortunately, most GUI libraries don't deal particularly well with multiple threads and usually have
some limitations with multiple thread access. WinForms is one such library. With WinForms, a Control can only be modified
on the thread that it was created on. The regular AnimatedObject class has no way of dealing with this.

In order to animate any type of WinForms control, you will need to use the **AnimatedControl** class. The AnimatedControl class
is a subclass of the AnimatedObject class that works specifically with Controls and takes extra precautions to ensure that the
wrapped Control is only ever modified on its creating thread. All of this is taken care of in the class. That means that the
AnimatedControl class functions exactly like the AnimatedObject class. You just need to remember to use AnimatedControl rather
than AnimatedObject.

You can use the following code to animate a label's X-position and background color:

```C#
var label = new Label();
label.Text = "Weeeee!";

var animatedLabel = new AnimatedControl(label);
animatedLabel.AnimatePropertyTo("Left", 500);
animatedLabel.AnimatePropertyTo("BackColor", Color.Blue);
```

_______________________________________________________________________________________________________________

### Other Features

This library is far from complete. While a lot of key functionality is already available, there are still issues to be addressed
and features to add. There are quite a few features that are already in the library but simply aren't written about here (however,
they are described in the XML Documentation comments in the code).
