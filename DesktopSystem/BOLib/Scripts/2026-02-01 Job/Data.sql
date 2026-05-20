--select * from sys_formsettingIDDet(nolock)where  listID='frmMSTJobGridEst' and ColFldNm='EstItmID'

--select * from sys_formsettingIDDet(nolock)where  listID='frmARQOGridItm' and userkey=0 and ColListID<>''


update sys_formsettingIDDet set ColListID='MSTItmSalesFunc_id'  where  listID='frmMSTJobGridEst' and ColFldNm='EstItmID'