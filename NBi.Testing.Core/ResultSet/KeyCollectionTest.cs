using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NBi.Core.ResultSet;

namespace NBi.Core.Testing.ResultSet;

public class KeyCollectionTest
{
    [Test]
    public void KeyCollection_Equal_True()
    {
        var a1 = new KeyCollection ([100, "a", true, new DateTime(2015,05,12)]);
        var a2 = new KeyCollection([100, "a", true, new DateTime(2015, 05, 12)]);

        var dico = new Dictionary<KeyCollection, object?>
        {
            { a1, null }
        };

        Assert.That(a1, Is.EqualTo(a2));
        Assert.That(a1.GetHashCode(), Is.EqualTo(a2.GetHashCode()));
        Assert.That(dico.ContainsKey(a2), Is.True);
    }

    [Test]
    public void KeyCollectionWithNull_Equal_True()
    {
        var a1 = new KeyCollection([100, DBNull.Value, true, new DateTime(2015, 05, 12)]);
        var a2 = new KeyCollection([100, "(null)", true, new DateTime(2015, 05, 12)]);

        var dico = new Dictionary<KeyCollection, object?>
        {
            { a1, null }
        };

        Assert.That(a1, Is.EqualTo(a2));
        Assert.That(a1.GetHashCode(), Is.EqualTo(a2.GetHashCode()));
        Assert.That(dico.ContainsKey(a2), Is.True);
    }

    [Test]
    public void KeyCollectionWithEmpty_Equal_True()
    {
        var a1 = new KeyCollection([100, string.Empty, true, new DateTime(2015, 05, 12)]);
        var a2 = new KeyCollection([100, "(empty)", true, new DateTime(2015, 05, 12)]);

        var dico = new Dictionary<KeyCollection, object?>
        {
            { a1, null }
        };

        Assert.That(a1, Is.EqualTo(a2));
        Assert.That(a1.GetHashCode(), Is.EqualTo(a2.GetHashCode()));
        Assert.That(dico.ContainsKey(a2), Is.True);
    }

    [Test]
    public void KeyCollection_Equal_False()
    {
        var a1 = new KeyCollection([100, "a", true, new DateTime(2015, 05, 12)]);
        var a2 = new KeyCollection([100, "a", false, new DateTime(2015, 05, 12)]);

        var dico = new Dictionary<KeyCollection, object?>
        {
            { a1, null }
        };

        Assert.That(a1, Is.Not.EqualTo(a2));
        Assert.That(a1.GetHashCode(), Is.Not.EqualTo(a2.GetHashCode()));
        Assert.That(dico.ContainsKey(a2), Is.Not.True);
    }

    [Test]
    public void KeyCollectionWithNullVersusEmpty_Equal_False()
    {
        var a1 = new KeyCollection([100, DBNull.Value, true, new DateTime(2015, 05, 12)]);
        var a2 = new KeyCollection([100, string.Empty, true, new DateTime(2015, 05, 12)]);

        var dico = new Dictionary<KeyCollection, object?>
        {
            { a1, null }
        };

        Assert.That(a1, Is.Not.EqualTo(a2));
        Assert.That(a1.GetHashCode(), Is.Not.EqualTo(a2.GetHashCode()));
        Assert.That(dico.ContainsKey(a2), Is.Not.True);
    }
}
