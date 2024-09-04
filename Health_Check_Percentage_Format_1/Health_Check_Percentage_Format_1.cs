/*
****************************************************************************
*  Copyright (c) 2024,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

03/09/2024	1.0.0.1		DPR, Skyline	Initial version
****************************************************************************
*/

namespace HealthCheckPercentageFormat_1
{
	using System;
	using System.Globalization;

	using Skyline.DataMiner.Analytics.GenericInterface;

	[GQIMetaData(Name = "Health_Check_Percentage_Format")]
	public class HealthCheckPercentageFormat : IGQIInputArguments, IGQIColumnOperator, IGQIRowOperator
	{
		private readonly GQIColumnDropdownArgument _lastRunColumnArg; // Input Argument to be requested from the user;
		private readonly GQIColumnDropdownArgument _longDurationColumnArg; // Input Argument to be requested from the user;

		private GQIColumn _lastRun;
		private GQIColumn _longDuration;

		private GQIStringColumn _lastRunResult;
		private GQIStringColumn _longDurationResult;

		public HealthCheckPercentageFormat()
		{
			_lastRunColumnArg = new GQIColumnDropdownArgument("Overall Failure % (Last Run)")
			{
				IsRequired = true,
				Types = new[] { GQIColumnType.Double },
			};

			_longDurationColumnArg = new GQIColumnDropdownArgument("Overall Failure % (Long Duration)")
			{
				IsRequired = true,
				Types = new[] { GQIColumnType.Double },
			};
		}

		public GQIArgument[] GetInputArguments()
		{
			return new GQIArgument[] { _lastRunColumnArg, _longDurationColumnArg };
		}

		public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
		{
			_lastRun = args.GetArgumentValue(_lastRunColumnArg);
			_longDuration = args.GetArgumentValue(_longDurationColumnArg);

			_lastRunResult = new GQIStringColumn("Overall Failure % (Last Run)");
			_longDurationResult = new GQIStringColumn("Overall Failure % (Long Duration)");

			return default;
		}

		public void HandleColumns(GQIEditableHeader header)
		{
			header.AddColumns(_lastRunResult);
			header.AddColumns(_longDurationResult);
		}

		public void HandleRow(GQIEditableRow row)
		{
			double lr = row.GetValue<double>(_lastRun);
			double ld = row.GetValue<double>(_longDuration);

			row.SetValue(_lastRunResult, GetPercentage(lr));
			row.SetValue(_longDurationResult, GetPercentage(ld));
		}

		private static string GetPercentage(double percentage)
		{
			return $"{Math.Round(percentage, 2)} %";
		}
	}
}
