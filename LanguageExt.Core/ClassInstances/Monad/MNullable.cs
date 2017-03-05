﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt.TypeClasses;
using System.Diagnostics.Contracts;
using static LanguageExt.Prelude;

namespace LanguageExt.ClassInstances
{
    public struct MNullable<A> :
        Optional<A?, A>,
        Monad<A?, A>,
        Foldable<A?, A>,
        BiFoldable<A?, Unit, A>
        where A : struct
    {
        public static readonly MNullable<A> Inst = default(MNullable<A>);

        [Pure]
        public A? None => (A?)null;

        [Pure]
        public MB Bind<MONADB, MB, B>(A? ma, Func<A, MB> f) where MONADB : struct, Monad<Unit, Unit, MB, B> =>
            ma.HasValue && f != null
                ? f(ma.Value)
                : default(MONADB).Fail(ValueIsNoneException.Default);

        [Pure]
        public A? Fail(object err) =>
            null;

        [Pure]
        public A? Fail(Exception err = null) =>
            null;

        [Pure]
        public A? Plus(A? a, A? b) =>
            a.HasValue
                ? a
                : b;

        [Pure]
        public A? Return(Func<Unit, A> f)
        {
            var x = f(unit);
            return isnull(x)
                ? null
                : (A?)f(unit);
        }

        [Pure]
        public A? Zero() =>
            null;

        [Pure]
        public bool IsNone(A? ma) =>
            !ma.HasValue;

        [Pure]
        public bool IsSome(A? ma) =>
            ma.HasValue;

        [Pure]
        public bool IsUnsafe(A? ma) =>
            true;

        [Pure]
        public B Match<B>(A? ma, Func<A, B> Some, Func<B> None) =>
            ma.HasValue
                ? Check.NullReturn(Some(ma.Value))
                : Check.NullReturn(None());

        public Unit Match(A? ma, Action<A> Some, Action None)
        {
            if (ma.HasValue) Some(ma.Value); else None();
            return Unit.Default;
        }

        [Pure]
        public B MatchUnsafe<B>(A? ma, Func<A, B> Some, Func<B> None) =>
            ma.HasValue
                ? Some(ma.Value)
                : None();

        [Pure]
        public Func<Unit, S> Fold<S>(A? ma, S state, Func<S, A, S> f) => _ =>
            Check.NullReturn(ma.HasValue
                ? f(state, ma.Value)
                : state);

        [Pure]
        public Func<Unit, S> FoldBack<S>(A? ma, S state, Func<S, A, S> f) => _ =>
            Check.NullReturn(ma.HasValue
                ? f(state, ma.Value)
                : state);

        [Pure]
        public S BiFold<S>(A? ma, S state, Func<S, Unit, S> fa, Func<S, A, S> fb) =>
            Check.NullReturn(!ma.HasValue
                ? fa(state, unit)
                : fb(state, ma.Value));

        [Pure]
        public S BiFoldBack<S>(A? ma, S state, Func<S, Unit, S> fa, Func<S, A, S> fb) =>
            Check.NullReturn(!ma.HasValue
                ? fa(state, unit)
                : fb(state, ma.Value));

        [Pure]
        public Func<Unit, int> Count(A? ma) => _ =>
            ma.HasValue
                ? 1
                : 0;

        [Pure]
        public A? Some(A value) =>
            value;

        [Pure]
        public A? Optional(A value) =>
            value;

        [Pure]
        public A? Id(Func<Unit, A?> ma) =>
            ma(unit);

        [Pure]
        public A? BindReturn(Unit _, A? mb) =>
            mb;

        [Pure]
        public A? Return(A x) =>
            Return(_ => x);
    }
}
