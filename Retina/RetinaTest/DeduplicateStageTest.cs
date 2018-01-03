﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RetinaTest
{
    [TestClass]
    public class DeduplicateStageTest : RetinaTestBase
    {
        [TestMethod]
        public void TestBasicDeduplication()
        {
            AssertProgram(new TestSuite { Sources = { @"D`." }, TestCases = { { "abacbcedef", "abcedf" } } });
            AssertProgram(new TestSuite { Sources = { @"D`\w+" }, TestCases = { { "abc def abc ab ghi def", "abc def  ab ghi " } } });
        }

        [TestMethod]
        public void TestRTLMatching()
        {
            AssertProgram(new TestSuite { Sources = { @"Dr`." }, TestCases = { { "abacbcedef", "abcedf" } } });
            AssertProgram(new TestSuite { Sources = { @"Dr`\w+" }, TestCases = { { "abc def abc ab ghi def", "abc def  ab ghi " } } });
        }

        [TestMethod]
        public void TestDeduplicateBy()
        {
            AssertProgram(new TestSuite { Sources = { @"D$`(\w)\w*", "$1" }, TestCases = { { "abc def abc ab ghi def", "abc def   ghi " } } });
        }

        [TestMethod]
        public void TestDefaultRegex()
        {
            AssertProgram(new TestSuite { Sources = { @"D`" }, TestCases = { { "abc\ndef\nabc\nab\nghi\ndef", "abc\ndef\n\nab\nghi\n" } } });
            AssertProgram(new TestSuite { Sources = { @"Dr`" }, TestCases = { { "abc\ndef\nabc\nab\nghi\ndef", "abc\ndef\n\nab\nghi\n" } } });
            AssertProgram(new TestSuite { Sources = { @"D$`", "$.&" }, TestCases = { { "abc\ndef\nabc\nab\nghi\ndef", "abc\n\n\nab\n\n" } } });
        }

        [TestMethod]
        public void TestOverlappingMatches()
        {
            AssertProgram(new TestSuite { Sources = { @"Dv`.+" }, TestCases = { { "aaaa", "aaaa" } } });
            AssertProgram(new TestSuite { Sources = { @"Drv`.+" }, TestCases = { { "aaaa", "aaaa" } } });
            AssertProgram(new TestSuite { Sources = { @"Dw`.+" }, TestCases = { { "aaaa", "a" } } });
            AssertProgram(new TestSuite { Sources = { @"Drw`.+" }, TestCases = { { "aaaa", "a" } } });
            AssertProgram(new TestSuite { Sources = { @"Dw`.." }, TestCases = { { "abc,ab,bc", "abc,," } } });
            AssertProgram(new TestSuite { Sources = { @"Dw`.." }, TestCases = { { "ab,bc,abc", "ab,bc," } } });
        }

        [TestMethod]
        public void TestKeepLast()
        {
            AssertProgram(new TestSuite { Sources = { @"D^`." }, TestCases = { { "abacbcedef", "abcdef" } } });
            AssertProgram(new TestSuite { Sources = { @"D^`\w+" }, TestCases = { { "abc def abc ab ghi def", "  abc ab ghi def" } } });
            AssertProgram(new TestSuite { Sources = { @"D^r`." }, TestCases = { { "abacbcedef", "abcdef" } } });
            AssertProgram(new TestSuite { Sources = { @"D^r`\w+" }, TestCases = { { "abc def abc ab ghi def", "  abc ab ghi def" } } });
            AssertProgram(new TestSuite { Sources = { @"D^w`.." }, TestCases = { { "abc,ab,bc", ",ab,bc" } } });
            AssertProgram(new TestSuite { Sources = { @"D^w`.." }, TestCases = { { "ab,bc,abc", ",,abc" } } });
        }
    }
}