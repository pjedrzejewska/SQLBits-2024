// If you have any questions: https://www.linkedin.com/in/paulina-jedrzejewska-a5b64a163/

//"Hello World".Output();

/* Generate measures from columns */

var MeasuresTable = Model.Tables["_Measures"];
var kpiSourceTable = Model.Tables["Sales"];

foreach(var column in kpiSourceTable.Columns)
{
    if (column.Name.Contains("KPI ")) 
    {
        var new_measure = MeasuresTable.AddMeasure(
        column.Name,                                    // Name
        "SUM(" + column.DaxObjectFullName + ")",        // DAX expression
        "Generate from Columns"                         // Display Folder
        );
    }
}

/* Create measures from calculation items */

var MeasuresTable = Model.Tables["_Measures"];
var baseMeasures = MeasuresTable.Measures.Where(m => (m.DisplayFolder=="Generate from Columns\\BASE")).ToList();

foreach(var measure in baseMeasures)
{
    // get correct measure name
    var correctMeasureName = measure.Name.Remove(0,3);
    
    foreach(var calcItem in (Model.Tables["Time Intelligence"] as CalculationGroupTable).CalculationItems)
    {
        // add measure
        var new_measure = MeasuresTable.AddMeasure(
        correctMeasureName + " " + calcItem.Name,                              // Name
        "CALCULATE(" + measure.DaxObjectFullName + ", 'Time Intelligence'[TimeIntelligence]=\"" + calcItem.Name + "\")",   // DAX expression
        "Generate from calculation items\\" + calcItem.Name                         // Display Folder
    );
    }
}

/* Create measures from calculation items + Description*/

var MeasuresTable = Model.Tables["_Measures"];
var baseMeasures = MeasuresTable.Measures.Where(m => (m.DisplayFolder=="Generate from Columns\\BASE")).ToList();

foreach(var measure in baseMeasures)
{
    // get correct measure name
    var correctMeasureName = measure.Name.Remove(0,3);
    
    foreach(var calcItem in (Model.Tables["Time Intelligence"] as CalculationGroupTable).CalculationItems)
    {
        // add measure
        var new_measure = MeasuresTable.AddMeasure(
        correctMeasureName + " " + calcItem.Name,                              // Name
        "CALCULATE(" + measure.DaxObjectFullName + ", 'Time Intelligence'[TimeIntelligence]=\"" + calcItem.Name + "\")",   // DAX expression
        "Generate from calculation items\\" + calcItem.Name                         // Display Folder
        );
        new_measure.Description = "BASE: " + measure.Expression + @"
        
CALC: " + calcItem.Expression;
    }
}



/* Calculation Group

AC
SELECTEDMEASURE()

PY
CALCULATE(SELECTEDMEASURE(), SAMEPERIODLASTYEAR('Date'[Date]))


AC-PY %
VAR ac_ = CALCULATE(SELECTEDMEASURE(), 'Time Intelligence'[TimeIntelligence]= "AC")
VAR py_ = CALCULATE(SELECTEDMEASURE(), 'Time Intelligence'[TimeIntelligence]= "PY")

RETURN
DIVIDE(ac_ - py_, py_)


PM
CALCULATE(SELECTEDMEASURE(), PREVIOUSMONTH('Date'[Date]))

/*