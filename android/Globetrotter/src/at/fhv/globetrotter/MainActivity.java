package at.fhv.globetrotter;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

public class MainActivity extends Activity {

	private static final String PATTERN = "^([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\."
			+ "([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\."
			+ "([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\."
			+ "([01]?\\d\\d?|2[0-4]\\d|25[0-5])$";

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		TextView ip = (TextView) findViewById(R.id.editIp);
		ip.setText("10.0.0.7");
		TextView port = (TextView) findViewById(R.id.editPort);
		port.setText("45021");
	}

	public static boolean CheckIp(final String ip) {
		Pattern pattern = Pattern.compile(PATTERN);
		Matcher matcher = pattern.matcher(ip);
		return matcher.matches();
	}

	public void scanQr(View view) {
		Intent intent = new Intent("com.google.zxing.client.android.SCAN");
		intent.putExtra("SCAN_MODE", "QR_CODE_MODE");
		startActivityForResult(intent, 0);
	}

	public void connect(View view) {
		String ip = ((TextView) findViewById(R.id.editIp)).getText().toString();

		String port = ((TextView) findViewById(R.id.editPort)).getText()
				.toString();

		if (CheckIp(ip)) {
			connect(ip, Integer.parseInt(port));
		}
	}

	public void connect(String ip, int port) {
		if (connectionTest(ip, port)) {
			Intent intentSender = new Intent(getApplicationContext(),
					SenderActivity.class);
			Bundle b = new Bundle();
			b.putString("ip", ip);
			b.putInt("port", port);
			intentSender.putExtras(b);
			startActivityForResult(intentSender, 0);
		}
	}

	public boolean connectionTest(String ip, int port) {
		return true;
	}

	public void onActivityResult(int requestCode, int resultCode, Intent intent) {
		if (requestCode == 0) {
			if (resultCode == RESULT_OK) {
				String[] temp = intent.getStringExtra("SCAN_RESULT").split(";");
				if (temp.length == 1) {
					Toast toast = Toast.makeText(getApplicationContext(),
							"Invalid information!", Toast.LENGTH_SHORT);
					toast.show();
				} else {
					try {
						if (CheckIp(temp[0])) {
							connect(temp[0], Integer.parseInt(temp[1]));
						}
					} catch (NumberFormatException e) {
						Toast toast = Toast.makeText(getApplicationContext(),
								"Invalid information!", Toast.LENGTH_SHORT);
						toast.show();
					}
				}
			} else if (resultCode == RESULT_CANCELED) {
				Toast toast = Toast.makeText(getApplicationContext(),
						"connection interrupted", Toast.LENGTH_SHORT);
				toast.show();
			}
		}
	}

}
