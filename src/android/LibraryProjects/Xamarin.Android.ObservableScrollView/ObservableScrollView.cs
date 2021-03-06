﻿using Context = Android.Content.Context;
using IAttributeSet = Android.Util.IAttributeSet;
using IParcelable = Android.OS.IParcelable;
using MotionEvent = Android.Views.MotionEvent;
using MotionEventActions = Android.Views.MotionEventActions;
using ScrollView = Android.Widget.ScrollView;
using View = Android.Views.View;
using ViewGroup = Android.Views.ViewGroup;

namespace Xamarin.Android.ObservableScrollView
{
  public class ObservableScrollView : ScrollView, IScrollable
	{
		// Fields that should be saved onSaveInstanceState
		int mPrevScrollY;
		int mScrollY;

		// Fields that don't need to be saved onSaveInstanceState
		IObservableScrollViewCallbacks mCallbacks;
		ScrollState mScrollState;
		bool mFirstScroll;
		bool mDragging;
		bool mIntercepted;
		MotionEvent mPrevMoveEvent;
		ViewGroup mTouchInterceptionViewGroup;

		public ObservableScrollView (Context context) :
			base (context)
		{
		}

		public ObservableScrollView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
		}

		public ObservableScrollView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
		}
			
		public int CurrentScrollY { get { return this.mScrollY; } }

		public void ScrollVerticallyTo (int y)
		{	
			this.ScrollTo (0, y);
		}

		protected override void OnRestoreInstanceState (IParcelable state)
		{
			var ss = (SimpleSavedState)state;	
			this.mPrevScrollY = ss.PrevScrollY;
			this.mScrollY = ss.ScrollY;
			base.OnRestoreInstanceState (ss.SuperState);
		}

		protected override IParcelable OnSaveInstanceState ()
		{
			IParcelable superState = base.OnSaveInstanceState ();
			var ss = new SimpleSavedState (superState);
			ss.PrevScrollY = this.mPrevScrollY;
			ss.ScrollY = this.mScrollY;
			return ss;
		}

		protected override void OnScrollChanged (int l, int t, int oldl, int oldt)
		{
			base.OnScrollChanged(l, t, oldl, oldt);
			if (this.mCallbacks != null) {
				this.mScrollY = t;

				this.mCallbacks.OnScrollChanged(t, this.mFirstScroll, this.mDragging);
				if (this.mFirstScroll) {
					this.mFirstScroll = false;
				}

				if (this.mPrevScrollY < t) {
					this.mScrollState = ScrollState.Up;
				} else if (t < this.mPrevScrollY) {
					this.mScrollState = ScrollState.Down;
					//} else {
					// Keep previous state while dragging.
					// Never makes it STOP even if scrollY not changed.
					// Before Android 4.4, onTouchEvent calls onScrollChanged directly for ACTION_MOVE,
					// which makes mScrollState always STOP when onUpOrCancelMotionEvent is called.
					// STOP state is now meaningless for ScrollView.
				}
				this.mPrevScrollY = t;
			}
		}

		public override bool OnInterceptTouchEvent (MotionEvent ev)
		{
			if (this.mCallbacks != null) {
				switch (ev.ActionMasked) {
				case MotionEventActions.Down:
					// Whether or not motion events are consumed by children,
					// flag initializations which are related to ACTION_DOWN events should be executed.
					// Because if the ACTION_DOWN is consumed by children and only ACTION_MOVEs are
					// passed to parent (this view), the flags will be invalid.
					// Also, applications might implement initialization codes to onDownMotionEvent,
					// so call it here.
					this.mFirstScroll = this.mDragging = true;
					this.mCallbacks.OnDownMotionEvent ();
					break;
				}
			}
			return base.OnInterceptTouchEvent (ev);
		}

		public override bool OnTouchEvent (MotionEvent ev)
		{
			if (this.mCallbacks == null)
				return base.OnTouchEvent (ev);

			switch (ev.ActionMasked) {
			case MotionEventActions.Up:
			case MotionEventActions.Cancel:
				this.mIntercepted = false;
				this.mDragging = false;
				this.mCallbacks.OnUpOrCancelMotionEvent (this.mScrollState);
				break;
			case MotionEventActions.Move:
				if (this.mPrevMoveEvent == null) {
					this.mPrevMoveEvent = ev;
				} 
				float diffY = ev.GetY () - this.mPrevMoveEvent.GetY ();
				this.mPrevMoveEvent = MotionEvent.ObtainNoHistory (ev);

				if (this.CurrentScrollY - diffY <= 0) {
					// Can't scroll anymore.

					if (this.mIntercepted) {
						// Already dispatched ACTION_DOWN event to parents, so stop here.
						return false;
					}

					// Apps can set the interception target other than the direct parent.
					ViewGroup parent;
					if (this.mTouchInterceptionViewGroup == null) {
						parent = (ViewGroup)this.Parent;
					} else {
						parent = this.mTouchInterceptionViewGroup;
					}

					// Get offset to parents. If the parent is not the direct parent,
					// we should aggregate offsets from all of the parents.
					float offsetX = 0;
					float offsetY = 0;
					for (View v = this; v != null && v != parent; v = (View)v.Parent) {
						offsetX += v.Left - v.ScrollX;
						offsetY += v.Top - v.ScrollY;
					}
					MotionEvent @event = MotionEvent.ObtainNoHistory (ev);
					@event.OffsetLocation (offsetX, offsetY);

					if (parent.OnInterceptTouchEvent (@event)) {
						this.mIntercepted = true;

						// If the parent wants to intercept ACTION_MOVE events,
						// we pass ACTION_DOWN event to the parent
						// as if these touch events just have began now.
						@event.Action = (MotionEventActions.Down);

						this.Post (() => parent.DispatchTouchEvent (@event));
						// Return this onTouchEvent() first and set ACTION_DOWN event for parent
						// to the queue, to keep events sequence.

						return false;
					}
					// Even when this can't be scrolled anymore,
					// simply returning false here may cause subView's click,
					// so delegate it to super.
					return base.OnTouchEvent (ev);
				}
				break;
			}

			return base.OnTouchEvent (ev);
		}
			

		public void SetTouchInterceptionViewGroup(ViewGroup viewGroup) {
			this.mTouchInterceptionViewGroup = viewGroup;
		}

		public void SetScrollViewCallbacks(IObservableScrollViewCallbacks listener) {
			this.mCallbacks = listener;
		}
	}
}

