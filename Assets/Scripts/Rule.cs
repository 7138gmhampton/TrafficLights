﻿internal abstract class Rule
{
    protected int priority;
    protected int xPosition;
    protected int yPosition;

    public int Priority { get { return priority; } }
    public int XPosition { get { return xPosition; } }
    public int YPosition { get { return yPosition; } }

    public abstract bool match(params object[] args);
    public abstract void fire();
}