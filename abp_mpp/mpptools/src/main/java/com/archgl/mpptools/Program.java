package com.archgl.mpptools;

import java.io.IOException;
import java.util.Date;

import org.apache.commons.cli.DefaultParser;
import org.apache.commons.cli.CommandLine;
import org.apache.commons.cli.CommandLineParser;
import org.apache.commons.cli.Options;
import org.apache.commons.cli.ParseException;
import org.junit.Test;

import net.sf.mpxj.MPXJException;

public class Program {
	public static void main(String[] args) {
		// Add program arguments
		CommandLineParser parser = new DefaultParser();
		Options options = new Options();
		options.addOption("h", "help", false, "Print this usage information");
		options.addOption("p", "password", true, "only needed if input file format is mpp and password protected");
		options.addOption("f", "force", false, "Overwrite output file if already exist");
		options.addOption("o", "origin", true, "the origin file");
		options.addOption("d", "destination", true, "the destination file");
		// Parse the program arguments
		CommandLine commandLine;
		try {
			commandLine = parser.parse(options, args);
			String password = null;
			if (commandLine.hasOption("p")) {
				password = commandLine.getOptionValue("p");
			}

			if (commandLine.hasOption("h")) {
				System.out.println(" -p 123456 -f -o e:\\a.mpp -d e:\\a.xml");
				System.exit(0);
			}
			
			Boolean force = false;
			if (commandLine.hasOption("f")) {
				force = true;
			}

			String originPath = null;
			if (commandLine.hasOption("o")) {
				originPath = commandLine.getOptionValue("o");
				System.out.println(originPath);
			}
			else {
				throw new IllegalArgumentException("the origin path is null");
			}

			String destinationPath = null;
			if (commandLine.hasOption("d")) {
				destinationPath = commandLine.getOptionValue("d");
				System.out.println(destinationPath);
			}
			else {
				throw new IllegalArgumentException("the destination path is null");
			}

			MppConvert.ToXMLByMSPDI(originPath, destinationPath, password);
			System.out.println(0);
			System.exit(0);
			
		} catch (ParseException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.exit(1);
		} catch (MPXJException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.exit(2);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.exit(3);
		} catch(IllegalArgumentException e) {
			e.printStackTrace();
			System.exit(4);
		}
	}

//	@Test
//	public void sayHello() {
//		try {
//			Date now = new Date();
//			MppConvert.ToXMLByMSPDI("D:\\Projects\\mpptools\\release\\A.mpp",
//					"D:\\Projects\\mpptools\\release\\" + now.getTime() + ".xml", null);
//
//		} catch (MPXJException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		} catch (IOException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
//	}
}
