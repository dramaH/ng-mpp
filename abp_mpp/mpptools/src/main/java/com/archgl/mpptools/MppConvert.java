package com.archgl.mpptools;

import java.io.IOException;

import net.sf.mpxj.MPXJException;
import net.sf.mpxj.ProjectFile;
import net.sf.mpxj.mpp.MPPReader;
import net.sf.mpxj.mspdi.MSPDIWriter;
import net.sf.mpxj.planner.PlannerWriter;
import net.sf.mpxj.primavera.PrimaveraPMFileWriter;
import net.sf.mpxj.writer.ProjectWriter;

/**
 * @author Tom
 * @version v1.0.0 MPP转换XML文件
 */
public class MppConvert {
	/**
	 * The MSPDI file format is Microsoft’s XML file format for storing project
	 * data. Versions of Microsoft Project from 2002 onwards can read and write
	 * MSPDI files. MPXJ allows MSPDI files to be created, read, and written. The
	 * MSDPI file format has remained broadly unchanged since it was introduced,
	 * although several versions of Microsoft Project have tweaked the file format
	 * slightly, and have their own updated documentation.
	 * 
	 * @param inputFile  输入文件
	 * @param outputFile 输出文件
	 * @throws MPXJException
	 * @throws IOException
	 */
	public static void ToXMLByMSPDI(String inputFile, String outputFile, String password)
			throws MPXJException, IOException {
		MPPReader reader = new MPPReader();
		if (password != null && password != "") {
			reader.setReadPassword(password);
		}
		
		ProjectFile projectFile = reader.read(inputFile);
		ProjectWriter writer = new MSPDIWriter();
		writer.write(projectFile, outputFile);
	}

	/**
	 * Primavera P6 is an industry-leading tool favoured by users with complex
	 * planning requirements. It can export project data in the form of XER or PMXML
	 * files, both of which MPXJ can read. It is also possible for MPXJ to connect
	 * directly to the P6 database via JDBC to read project data. MPXJ can also
	 * write PMXML files to allow data to be exported in a form which can be
	 * consumed by P6. The PMXML schema forms part of the P6 distribution media,
	 * which can be downloaded from the Oracle e-Delivery site
	 * 
	 * @param inputFile  输入文件
	 * @param outputFile 输出文件
	 * @throws MPXJException
	 * @throws IOException
	 */
	public static void ToXMLByPPMF(String inputFile, String outputFile, String password)
			throws MPXJException, IOException {
		MPPReader reader = new MPPReader();
		if (password != null && password != "") {
			reader.setReadPassword(password);
		}

		ProjectFile projectFile = reader.read(inputFile);
		ProjectWriter writer = new PrimaveraPMFileWriter();
		writer.write(projectFile, outputFile);
	}

	/**
	 * Planner is an Open Source project management tool which uses an XML file
	 * format to store project data. MPXJ can read and write the Planner file
	 * format.
	 * 
	 * @param inputFile  输入文件
	 * @param outputFile 输出文件
	 * @throws MPXJException
	 * @throws IOException
	 */
	public static void ToXMLByPlanner(String inputFile, String outputFile, String password)
			throws MPXJException, IOException {
		MPPReader reader = new MPPReader();
		if (password != null && password != "") {
			reader.setReadPassword(password);
		}

		ProjectFile projectFile = reader.read(inputFile);
		ProjectWriter writer = new PlannerWriter();
		writer.write(projectFile, outputFile);
	}
}