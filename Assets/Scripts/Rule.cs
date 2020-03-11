internal abstract class Rule
{
    protected int priority;
    protected int xPosition;
    protected int yPosition;

    public int Priority { get { return priority; } }

    public abstract bool match(object args);
    public abstract void fire();
}