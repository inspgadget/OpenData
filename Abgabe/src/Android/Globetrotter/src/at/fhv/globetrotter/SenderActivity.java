package at.fhv.globetrotter;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.text.SimpleDateFormat;
import java.util.GregorianCalendar;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v4.view.GestureDetectorCompat;
import android.util.Log;
import android.view.GestureDetector.OnDoubleTapListener;
import android.view.GestureDetector.OnGestureListener;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MotionEvent;
import android.view.ScaleGestureDetector;
import android.view.View;

public class SenderActivity extends Activity implements SensorEventListener,
		OnGestureListener, OnDoubleTapListener {

	static final String TAG = "at.fhv.globetrotter.SenderActivity";
	static final String SENSOR_TAG = "at.fhv.globetrotter.Sensors";

	Sender _sender;
	GestureSender _gestureSender;
	SensorManager _sensorManager;
	GestureDetectorCompat _gestureDetector;
	ScaleGestureDetector _scaleGestureDetector;
	Sensor _gyroSensor;
	boolean _connected = true;
	String _ipAddress;
	int _port;
	GregorianCalendar _timestamp = new GregorianCalendar();
	GregorianCalendar _timestampScale = new GregorianCalendar();
	float _sensitivityHorizontal = 30;
	float _sensitivityVertical = 30;
	float _sensitivity = 150;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Bundle b = getIntent().getExtras();
		_ipAddress = b.getString("ip");
		_port = b.getInt("port");
		setContentView(R.layout.activity_sender);
		_gestureDetector = new GestureDetectorCompat(this, this);
		_gestureDetector.setOnDoubleTapListener(this);
		_scaleGestureDetector = new ScaleGestureDetector(
				getApplicationContext(), new ScaleListener());
		_sensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
		_gyroSensor = _sensorManager
				.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
		_sensorManager.registerListener(this, _gyroSensor,
				SensorManager.SENSOR_DELAY_UI);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.sender, menu);
		return true;
	}

	private class Sender extends AsyncTask<float[], Object, Object> {
		private String _timestamp;

		public Sender(String timestamp) {
			_timestamp = timestamp;
		}

		@Override
		protected Object doInBackground(float[]... arg0) {
			try {
				DatagramSocket s = new DatagramSocket();
				InetAddress local = InetAddress.getByName(_ipAddress);
				String msg = ArrayToString(arg0[0], _timestamp);
				int msgLength = msg.length();
				byte[] message = msg.getBytes();
				DatagramPacket p = new DatagramPacket(message, msgLength,
						local, _port);
				s.send(p);
			} catch (UnknownHostException e) {
				Log.i(TAG, e.getMessage());
			} catch (IOException e) {
				Log.i(TAG, e.getMessage());
			}
			return null;
		}
	}

	private class GestureSender extends AsyncTask<Object, Object, Object> {

		@Override
		protected Object doInBackground(Object... arg0) {
			try {
				DatagramSocket s = new DatagramSocket();
				InetAddress local = InetAddress.getByName(_ipAddress);
				String msg = String.valueOf(arg0[0]);
				msg += "<EOF>";
				int msgLength = msg.length();
				byte[] message = msg.getBytes();
				DatagramPacket p = new DatagramPacket(message, msgLength,
						local, _port);
				s.send(p);
			} catch (UnknownHostException e) {
				Log.i(TAG, e.getMessage());
			} catch (IOException e) {
				Log.i(TAG, e.getMessage());
			}
			return null;
		}
	}

	public String ArrayToString(float[] array, String timestamp) {
		StringBuilder sb = new StringBuilder();
		sb.append("Acc;");
		for (float item : array) {
			sb.append(String.valueOf(item));
			sb.append(";");
		}
		sb.append(";");
		sb.append(timestamp.toString());
		sb.append("<EOF>");
		return sb.toString();
	}

	private class ScaleListener extends
			ScaleGestureDetector.SimpleOnScaleGestureListener {
		@Override
		public boolean onScale(ScaleGestureDetector detector) {
			GregorianCalendar currentTimestamp = new GregorianCalendar();
			if (currentTimestamp.getTimeInMillis()
					- _timestampScale.getTimeInMillis() >= 50) {
				_gestureSender = new GestureSender();
				_gestureSender.execute("Scale;" + detector.getScaleFactor());
				_timestampScale = currentTimestamp;
			}
			Log.i(SENSOR_TAG, "ScaleGesture");
			return true;
		}
	}

	@Override
	public void onSensorChanged(SensorEvent event) {
		if (_connected) {
			GregorianCalendar currentTimestamp = new GregorianCalendar();
			if (currentTimestamp.getTimeInMillis()
					- _timestamp.getTimeInMillis() >= 50) {
				SimpleDateFormat format = new SimpleDateFormat(
						"MM.dd.yyyy HH:mm:ss");
				int n = (int) currentTimestamp.getTimeInMillis() % 1000;
				n = n < 0 ? n + 1000 : n;
				_sender = new Sender(format.format(currentTimestamp.getTime())
						+ "." + n);
				_sender.execute(event.values);
				_timestamp = currentTimestamp;
			}

		}
	}

	@Override
	protected void onResume() {
		super.onResume();
		_sensorManager.registerListener(this, _gyroSensor,
				SensorManager.SENSOR_DELAY_UI);
	}

	@Override
	protected void onPause() {
		super.onPause();
		_sensorManager.unregisterListener(this);
	}

	@Override
	public boolean onTouchEvent(MotionEvent event) {
		if (_connected) {
			this._gestureDetector.onTouchEvent(event);
			this._scaleGestureDetector.onTouchEvent(event);
		}
		return super.onTouchEvent(event);
	}

	@Override
	public boolean onDown(MotionEvent event) {
		return true;
	}

	@Override
	public boolean onFling(MotionEvent event1, MotionEvent event2,
			float velocityX, float velocityY) {

		if ((event1.getX() - event2.getX()) > _sensitivityHorizontal
				&& Math.abs(event1.getY() - event2.getY()) <= _sensitivity) {
			if (_connected) {
				_gestureSender = new GestureSender();
				_gestureSender.execute("SwipeLeft");
				Log.i(SENSOR_TAG, "LeftSwipe");
			}
		} else if ((event1.getY() - event2.getY()) > _sensitivityVertical
				&& Math.abs(event1.getX() - event2.getX()) <= _sensitivity) {
			if (_connected) {
				_gestureSender = new GestureSender();
				_gestureSender.execute("UpSwipe");
				Log.i(SENSOR_TAG, "UpSwipe");
			}
		} else if ((event2.getX() - event1.getX()) > _sensitivityHorizontal
				&& Math.abs(event1.getY() - event2.getY()) <= _sensitivity) {
			if (_connected) {
				_gestureSender = new GestureSender();
				_gestureSender.execute("SwipeRight");
				Log.i(SENSOR_TAG, "RightSwipe");
			}
		} else if ((event2.getY() - event1.getY()) > _sensitivityVertical
				&& Math.abs(event1.getX() - event2.getX()) <= _sensitivity) {
			if (_connected) {
				_gestureSender = new GestureSender();
				_gestureSender.execute("DownSwipe");
				Log.i(SENSOR_TAG, "DownSwipe");
			}
		}
		return true;
	}

	@Override
	public void onLongPress(MotionEvent event) {
		if (_connected) {
			_gestureSender = new GestureSender();
			_gestureSender.execute("LongPress");
		}
		Log.i(SENSOR_TAG, "LongPress");
	}

	@Override
	public boolean onScroll(MotionEvent e1, MotionEvent e2, float distanceX,
			float distanceY) {
		return true;
	}

	@Override
	public void onShowPress(MotionEvent event) {
	}

	@Override
	public boolean onSingleTapUp(MotionEvent event) {
		return true;
	}

	@Override
	public boolean onDoubleTap(MotionEvent event) {
		if (_connected) {
			_gestureSender = new GestureSender();
			_gestureSender.execute("DoubleTap");
		}
		Log.i(SENSOR_TAG, "DoubleTap");
		return true;
	}

	@Override
	public boolean onDoubleTapEvent(MotionEvent event) {
		return true;
	}

	@Override
	public boolean onSingleTapConfirmed(MotionEvent event) {
		if (_connected) {
			_gestureSender = new GestureSender();
			_gestureSender.execute("SingleTap");
		}
		Log.i(SENSOR_TAG, "SingleTap");
		return true;
	}

	@Override
	public boolean dispatchKeyEvent(KeyEvent event) {
		int action = event.getAction();
		int keyCode = event.getKeyCode();
		switch (keyCode) {
		case KeyEvent.KEYCODE_VOLUME_UP:
			if (action == KeyEvent.ACTION_UP) {
				if (_connected) {
					_gestureSender = new GestureSender();
					_gestureSender.execute("VolumeUp");
				}
				Log.i(SENSOR_TAG, "VolumeUp");
			}
			return true;
		case KeyEvent.KEYCODE_VOLUME_DOWN:
			if (action == KeyEvent.ACTION_DOWN) {
				if (_connected) {
					_gestureSender = new GestureSender();
					_gestureSender.execute("VolumeDown");
				}
				Log.i(SENSOR_TAG, "VolumeDown");
			}
			return true;
		default:
			return super.dispatchKeyEvent(event);
		}
	}

	@Override
	public void onAccuracyChanged(Sensor sensor, int accuracy) {
		Log.i(SENSOR_TAG, "onAccuracyChanged: " + sensor.getName()
				+ ", accuracy: " + accuracy);
	}

	public void help(View view) {
		Intent intentHelp = new Intent(getApplicationContext(),
				HelpActivity.class);
		startActivityForResult(intentHelp, 0);
	}
}
