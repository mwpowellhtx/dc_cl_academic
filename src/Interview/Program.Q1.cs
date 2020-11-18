using System;
using System.Collections.Generic;
using System.Linq;

// InterviewZen: https://interviewzen.com/interview/58drMKF
namespace Academic.Shapes
{
    /*
        1. You are designing a .NET software application that can calculate the area of different
        kinds of geometric shapes. In the first version, you are told to support circle, square and
        rectangle but your design should be flexible to support other types of shapes in the future
        (and other types of calculations for the shapes). The area of a geometric shape depends on
        what kind of shape it is (for circle, area depends on the radius; for square and rectangle
        area depends on the length of the sides). Consumers of your classes need to be able to create
        circles, squares and rectangles of various sizes and be able to calculate the area of each.

        (a) Describe the class-level design for your solution. Your design should include
            i. any interfaces, abstract classes and concrete classes
            ii. instance variables in each class
            iii. definitions of constructors for each concrete class
            iv. definitions  of the method(s) for calculating the area of each kind of shape

        (b) Write C# code that will create a collection that will store all the objects created in
        part (a) and then sort that collection based on the area of each object.
     */

    /// <summary>
    /// Represents a <see cref="double"/> based <see cref="X"/> and <see cref="Y"/> Coordinate Point.
    /// </summary>
    public struct PointD
    {
        /// <summary>
        /// Gets or Sets the X Coordinate.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or Sets the Y Coordinate.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Constructs the <see cref="double"/> based Point given
        /// <paramref name="x"/> and <paramref name="y"/> Coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PointD(double x = default, double y = default)
        {
            this.X = x;
            this.Y = y;
        }
    }

    /// <summary>
    /// Shapes knows how to report their <see cref="Area"/>.
    /// </summary>
    /// <remarks>Note that for academic purposes we can simply ask the Area-comparable
    /// question with a direct <see cref="IComparable{T}"/> implementation on the
    /// <see cref="IShape"/> itself. This is perfectly adequate to the task at hand.
    /// However, for a more production-ready solution, we would want to consider
    /// leveraging constructs such as <see cref="Comparer{T}"/> implementations and
    /// related scaffold.</remarks>
    public interface IShape : IComparable<IShape>
    {
        /// <summary>
        /// Gets the Area for the Shape.
        /// </summary>
        double Area { get; }
    }

    /// <summary>
    /// Shape base class.
    /// </summary>
    public abstract class Shape : IShape
    {
        /// <summary>
        /// Gets or Sets the Location.
        /// </summary>
        /// <see cref="PointD"/>
        public PointD Location { get; set; } = new PointD();

        /// <inheritdoc/>
        public abstract double Area { get; }

        /// <summary>
        /// Constructs a Shape given <paramref name="location"/>.
        /// </summary>
        /// <param name="location">The <see cref="double"/> based Location of the Shape.</param>
        protected Shape(PointD location)
        {
            this.Location = location;
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected Shape()
        {
        }

        /// <inheritdoc/>
        public int CompareTo(IShape other)
        {
            // Other may be Null, so we account for that corner case.
            var delta = this.Area - other?.Area;

            // This is always greater than a Null instance.
            if (!delta.HasValue)
            {
                const int gt = 1;

                return gt;
            }

            /* For academic purposes this is sufficient to the task at hand, however,
             * for production code, we would want to consider things like comparer
             * classes or other scaffoldy sorts of constructs. */
            static int GetResult(double value, Func<double, double> callback) =>
                (int)callback.Invoke(value);

            var resultOrDefault = delta ?? default;

            /* Return either the Floor or the Ceiling, depending upon which direction
             * the result is away from zero, so as to ensure the most appropriate
             * comparison result not equal to zero (0). */
            if (delta < default(double))
            {
                return GetResult(resultOrDefault, Math.Floor);
            }

            if (delta > default(double))
            {
                return GetResult(resultOrDefault, Math.Ceiling);
            }

            // Otherwise return default, the two shapes are Area-equal.
            return default;
        }
    }

    /// <summary>
    /// Represents a Circle Shape.
    /// </summary>
    /// <remarks>Circle is analogous to <see cref="Square"/>, in the sense that
    /// it is a constrained form of an Ellipse, as <see cref="Square"/> is a
    /// constrained form of a <see cref="Rectangle"/>.</remarks>
    public class Circle : Shape
    {
        /// <summary>
        /// Gets or Sets the Circle Radius.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Gets the Area of the Circle, or &#960; (PI) times <see cref="Radius"/> squared.
        /// </summary>
        /// <see cref="!:https://en.wikipedia.org/wiki/Area_of_a_circle"/>
        /// <remarks>In a nutshell, this is it, though academic. In production code we might
        /// look to performance issues, perhaps clear a <see cref="double?"/> field when
        /// <see cref="Radius"/> changes, along these lines. But for illustration purposes,
        /// we Keep It Simple Stupid (KISS).</remarks>
        public override double Area => Math.PI * this.Radius * this.Radius;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Circle()
        {
        }

        /// <summary>
        /// Constructs a Circle given <see cref="PointD"/> <paramref name="location"/>.
        /// </summary>
        /// <param name="location"></param>
        public Circle(PointD location)
            : base(location)
        {
        }

        /// <summary>
        /// Constructs a Circle given <see cref="PointD"/> <paramref name="location"/> and <paramref name="radius"/>.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="radius"></param>
        public Circle(PointD location, double radius)
            : base(location)
        {
            this.Radius = radius;
        }
    }

    /// <summary>
    /// Represents a Rectangle Shape for academic purposes.
    /// </summary>
    /// <remarks>Although we might look at leveraging things like <see cref="System.Drawing.Rectangle"/>, for instance.</remarks>
    public class Rectangle : Shape
    {
        /// <summary>
        /// Gets the Area for the Rectangle as a function of <see cref="Height"/> times <see cref="Width"/>.
        /// </summary>
        /// <see cref="!:https://en.wikipedia.org/wiki/Rectangle#Formulae"/>
        public override double Area => this.Height * this.Width;

        public virtual PointD Size { get; set; }

        /// <summary>
        /// Gets or Sets the Height as a function of <see cref="Shape.Location"/> and <see cref="Bounds"/>.
        /// </summary>
        /// <remarks>Note that we calculate in relative terms. For <see cref="Area"/> calculations,
        /// we will want to consider <see cref="Math.Abs(double)"/> aspects.</remarks>
        public virtual double Height
        {
            get => this.Size.Y - this.Location.Y;
            set => this.Size = new PointD(this.Size.X, this.Location.Y + value);
        }

        /// <summary>
        /// Gets or Sets the Width as a function of <see cref="Shape.Location"/> and <see cref="Bounds"/>.
        /// </summary>
        /// <remarks>Note that we calculate in relative terms. For <see cref="Area"/> calculations,
        /// we will want to consider <see cref="Math.Abs(double)"/> aspects.</remarks>
        public virtual double Width
        {
            get => this.Size.X - this.Location.X;
            set => this.Size = new PointD(this.Location.X + value, this.Size.Y);
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <see cref="Rectangle(PointD)"/>
        public Rectangle()
            : this(default)
        {
        }

        /// <summary>
        /// Constructs a Rectangle with <paramref name="height"/> and <paramref name="width"/>.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <see cref="Rectangle(PointD, double, double)"/>
        public Rectangle(double width, double height)
            : this(default, width, height)
        {
        }

        /// <summary>
        /// Constructs a Rectangle with <paramref name="location"/> and default <see cref="Bounds"/>.
        /// </summary>
        /// <param name="location"></param>
        public Rectangle(PointD location)
            : base(location)
        {
            this.Size = default;
        }

        /// <summary>
        /// Constructs a Rectangle with <paramref name="location"/>, <paramref name="height"/> and <paramref name="width"/>.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(PointD location, double width, double height)
            : base(location)
        {
            this.Size = new PointD(this.Location.X + width, this.Location.Y + height);
        }
    }

    /// <summary>
    /// Square represents a specialized form of <see cref="Rectangle"/> in which both
    /// <see cref="Rectangle.Height"/> and <see cref="Rectangle.Width"/> are equal. The
    /// Square <see cref="IShape.Area"/> calculation is the same as the
    /// <see cref="Rectangle.Area"/> implementation. The only difference is in how we
    /// manage the dimensions of Square versus <see cref="Rectangle"/>.
    /// </summary>
    public class Square : Rectangle
    {
        /// <summary>
        /// Gets the Height based on the <see cref="Rectangle.Height"/>.
        /// When we Set the value, this also influences <see cref="Width"/>.
        /// </summary>
        /// <see cref="Rectangle.Size"/>
        /// <see cref="Rectangle.Height"/>
        /// <see cref="Width"/>
        public override double Height
        {
            get => base.Height;
            set => this.Size = new PointD(value, value);
        }

        /// <summary>
        /// Gets the Height based on the <see cref="Rectangle.Width"/>.
        /// When we Set the value, this also influences <see cref="Height"/>.
        /// </summary>
        /// <see cref="Rectangle.Size"/>
        /// <see cref="Rectangle.Width"/>
        /// <see cref="Height"/>
        public override double Width
        {
            get => base.Width;
            set => this.Size = new PointD(value, value);
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Square()
        {
        }

        /// <summary>
        /// Constructs a Square with <paramref name="size"/> <see cref="Height"/> and <see cref="Width"/>.
        /// </summary>
        /// <param name="size"></param>
        public Square(double size)
            : this(default, size)
        {
        }

        /// <summary>
        /// Constructs a Square with <paramref name="location"/>.
        /// </summary>
        /// <param name="location"></param>
        public Square(PointD location)
            : this(location, default)
        {
        }

        /// <summary>
        /// Constructs a Square with <paramref name="location"/> and <paramref name="size"/>.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        public Square(PointD location, double size)
            : base(location)
        {
            // Which also sets the Width.
            this.Height = size;
        }
    }

    public static class Program
    {
        /// <summary>
        /// Gets or Sets the Shapes for use throughout the example.
        /// </summary>
        internal static List<IShape> Shapes { get; set; } = Array.Empty<IShape>().ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="!:https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.sort#System_Collections_Generic_List_1_Sort"/>
        static void Main()
        {
            // TODO: TBD: add some Shapes to the collection...
            Shapes.Add(new Circle());
            Shapes.Add(new Circle(new PointD(2, 4)));
            Shapes.Add(new Rectangle());
            Shapes.Add(new Rectangle(2, 4));
            Shapes.Add(new Square());
            Shapes.Add(new Square(2));
            Shapes.Add(new Square(4));

            // Leverages the IComparable<IShape> implementation.
            Shapes.Sort();
        }
    }
}
