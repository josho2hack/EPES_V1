
function onExporting(e) {
	var workbook = new ExcelJS.Workbook();
	var worksheet = workbook.addWorksheet('Report');

	DevExpress.excelExporter.exportPivotGrid({
		component: e.component,
		worksheet: worksheet
	}).then(function () {
		workbook.xlsx.writeBuffer().then(function (buffer) {
			saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Report.xlsx');
		});
	});
	e.cancel = true;
}