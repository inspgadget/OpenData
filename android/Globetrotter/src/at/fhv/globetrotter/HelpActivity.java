package at.fhv.globetrotter;

import android.os.Bundle;
import android.app.Activity;
import android.view.Menu;

public class HelpActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_help);
		/*
		 * Intent intent = new Intent(Intent.ACTION_VIEW,
		 * Uri.parse(newVideoPath));
		 * intent.setDataAndType(Uri.parse(newVideoPath), "video/mp4");
		 * startActivity(intent);
		 */
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.help, menu);
		return true;
	}

}
