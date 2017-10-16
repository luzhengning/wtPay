using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPay
{
    public class ResourceInitialise
    {
        /// <summary>
        /// 加载页面
        /// </summary>
        public void LoadForm()
        {
            ResourceManager rm = ResourceManager.getInstance();
            //参数为（对象名称，类名全名）
            rm.addResource<wtPay.MainPage>("mainPage", "wtPay.MainPage");

            rm.addResource<wtPay.AdvertisePage>("advertisePage", "wtPay.AdvertisePage");
            rm.addResource<wtPay.FormCitizen.FormCitizenStep01>("formCitizenStep01", "wtPay.FormCitizen.FormCitizenStep01");//注意顺序，先载入内部frame
            //通用页面
            rm.addResource<wtPay.GeneralForm.FormFail>("FormFail", "wtPay.GeneralForm.FormFail");
            rm.addResource<wtPay.GeneralForm.FormFailRefund>("FormFailRefund", "wtPay.GeneralForm.FormFailRefund");
            rm.addResource<wtPay.GeneralForm.FormInputPassword>("FormInputPassword", "wtPay.GeneralForm.FormInputPassword");
            rm.addResource<wtPay.GeneralForm.FormReadCard>("FormReadCard", "wtPay.GeneralForm.FormReadCard");
            rm.addResource<wtPay.GeneralForm.FormNot>("FormNot", "wtPay.GeneralForm.FormNot");
            rm.addResource<wtPay.GeneralForm.FormFailNull>("FormFailNull", "wtPay.GeneralForm.FormFailNull");
            rm.addResource<wtPay.GeneralForm.FormCardFail>("FormCardFail", "wtPay.GeneralForm.FormCardFail");
            rm.addResource<wtPay.GeneralForm.FormPrintError>("FormPrintError", "wtPay.GeneralForm.FormPrintError");
            rm.addResource<wtPay.GeneralForm.FormIsContinue>("FormIsContinue", "wtPay.GeneralForm.FormIsContinue");
            rm.addResource<wtPay.GeneralForm.FormPayType>("FormPayType", "wtPay.GeneralForm.FormPayType");
            rm.addResource<wtPay.GeneralForm.FormCashPay>("FormCashPay", "wtPay.GeneralForm.FormCashPay");
            //惠民卡充值  
            rm.addResource<wtPay.FormCitizen.FormCitizenStep>("FormCitizenStep", "wtPay.FormCitizen.FormCitizenStep");
            rm.addResource<wtPay.FormCitizen.FormCitizenStepInputPwd>("FormCitizenStepInputPwd", "wtPay.FormCitizen.FormCitizenStepInputPwd");
            rm.addResource<wtPay.FormCitizen.FormCitizenStepValidatecode_1>("FormCitizenStepValidatecode_1", "wtPay.FormCitizen.FormCitizenStepValidatecode_1");
            rm.addResource<wtPay.FormCitizen.FormCitizenStepLoad>("FormCitizenStepLoad", "wtPay.FormCitizen.FormCitizenStepLoad");
            rm.addResource<wtPay.FormCitizen.FormCitizenStepSpendDetail>("FormCitizenStepSpendDetail", "wtPay.FormCitizen.FormCitizenStepSpendDetail");
            rm.addResource<wtPay.FormCitizen.FormCitizenStepRechargeDetail>("FormCitizenStepRechargeDetail", "wtPay.FormCitizen.FormCitizenStepRechargeDetail");
            rm.addResource<wtPay.FormCitizen.FormCitizenStepUpdatePwd>("FormCitizenStepUpdatePwd", "wtPay.FormCitizen.FormCitizenStepUpdatePwd");
            rm.addResource<wtPay.FormCitizen.FormCitizenStep01>("FormCitizenStep01", "wtPay.FormCitizen.FormCitizenStep01");
            rm.addResource<wtPay.FormCitizen.FormCitizenStep02>("FormCitizenStep02", "wtPay.FormCitizen.FormCitizenStep02");
            rm.addResource<wtPay.FormCitizen.FormCitizenStep03>("FormCitizenStep03", "wtPay.FormCitizen.FormCitizenStep03");
            rm.addResource<wtPay.FormCitizen.FormCitizenStep04>("FormCitizenStep04", "wtPay.FormCitizen.FormCitizenStep04");
            rm.addResource<wtPay.FormCitizen.FormCitizenStep07>("FormCitizenStep07", "wtPay.FormCitizen.FormCitizenStep07");
            rm.addResource<wtPay.FormCitizen.FormCitizenStep08_success>("FormCitizenStep08_success", "wtPay.FormCitizen.FormCitizenStep08_success");
            rm.addResource<wtPay.FormCitizen.FormCitizenInputNo>("FormCitizenInputNo", "wtPay.FormCitizen.FormCitizenInputNo");
            rm.addResource<wtPay.FormCitizen.FormSelectAmout>("FormSelectAmout", "wtPay.FormCitizen.FormSelectAmout");
            //移动页面 
            rm.addResource<wtPay.FormMobile.FormMobileStep>("FormMobileStep", "wtPay.FormMobile.FormMobileStep");
            rm.addResource<wtPay.FormMobile.FormMobileStep01>("FormMobileStep01", "wtPay.FormMobile.FormMobileStep01");
            rm.addResource<wtPay.FormMobile.FormMobileStep02>("FormMobileStep02", "wtPay.FormMobile.FormMobileStep02");
            rm.addResource<wtPay.FormMobile.FormMobileStep03>("FormMobileStep03", "wtPay.FormMobile.FormMobileStep03");
            rm.addResource<wtPay.FormMobile.FormMobileStep06>("FormMobileStep06", "wtPay.FormMobile.FormMobileStep06");
            rm.addResource<wtPay.FormMobile.FormMobileStep06_success>("FormMobileStep06_success", "wtPay.FormMobile.FormMobileStep06_success");
            rm.addResource<wtPay.FormMobile.FormMobileSelectAmout>("FormMobileSelectAmout", "wtPay.FormMobile.FormMobileSelectAmout");

            //公交卡 
            rm.addResource<wtPay.FormBus.FormBusStep01>("FormBusStep01", "wtPay.FormBus.FormBusStep01");
            rm.addResource<wtPay.FormBus.FormBusStep03>("FormBusStep03", "wtPay.FormBus.FormBusStep03");
            rm.addResource<wtPay.FormBus.FormBusStep04>("FormBusStep04", "wtPay.FormBus.FormBusStep04");
            rm.addResource<wtPay.FormBus.FormBusStep07>("FormBusStep07", "wtPay.FormBus.FormBusStep07");
            rm.addResource<wtPay.FormBus.FormBusStep08_success>("FormBusStep08_success", "wtPay.FormBus.FormBusStep08_success");

            //燃气 
            rm.addResource<wtPay.FormGas.FormGas>("FormGas", "wtPay.FormGas.FormGas");
            rm.addResource<wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep02>("FormGasGoldenCardStep02", "wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep02");
            rm.addResource<wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep03>("FormGasGoldenCardStep03", "wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep03");
            rm.addResource<wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep04>("FormGasGoldenCardStep04", "wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep04");
            rm.addResource<wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep07>("FormGasGoldenCardStep07", "wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep07");
            rm.addResource<wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep08_success>("FormGasGoldenCardStep08_success", "wtPay.FormGas.FormGasGoldenCard.FormGasGoldenCardStep08_success");

            rm.addResource<wtPay.FormGas.FromGasPioneerCard.FormGasPioneerCardStep02>("FormGasPioneerCardStep02", "wtPay.FormGas.FromGasPioneerCard.FormGasPioneerCardStep02");
            rm.addResource<wtPay.FormGas.FormGasGoldenCardFail>("FormGasGoldenCardFail", "wtPay.FormGas.FormGasGoldenCardFail");

            //联通页面
            rm.addResource<wtPay.FormUnicom.FormUnicomStep02>("FormUnicomStep02", "wtPay.FormUnicom.FormUnicomStep02");
            rm.addResource<wtPay.FormUnicom.FormUnicomStep03>("FormUnicomStep03", "wtPay.FormUnicom.FormUnicomStep03");
            rm.addResource<wtPay.FormUnicom.FormUnicomStep06>("FormUnicomStep06", "wtPay.FormUnicom.FormUnicomStep06");
            rm.addResource<wtPay.FormUnicom.FormUnicomStep06_success>("FormUnicomStep06_success", "wtPay.FormUnicom.FormUnicomStep06_success");
            //电力页面
            rm.addResource<wtPay.FormElectric.FormElectricStep01>("FormElectricStep01", "wtPay.FormElectric.FormElectricStep01");
            rm.addResource<wtPay.FormElectric.FormElectricStep02>("FormElectricStep02", "wtPay.FormElectric.FormElectricStep02");
            rm.addResource<wtPay.FormElectric.FormElectricStep03>("FormElectricStep03", "wtPay.FormElectric.FormElectricStep03");
            rm.addResource<wtPay.FormElectric.FormElectricStep06>("FormElectricStep06", "wtPay.FormElectric.FormElectricStep06");
            rm.addResource<wtPay.FormElectric.FormElectricStep06_success>("FormElectricStep06_success", "wtPay.FormElectric.FormElectricStep06_success");
            //公积金
            rm.addResource<wtPay.FormPublicFund.FormPublicFund>("FormPublicFund", "wtPay.FormPublicFund.FormPublicFund");
            rm.addResource<wtPay.FormPublicFund.FormPublicFundAccountInfo>("FormPublicFundAccountInfo", "wtPay.FormPublicFund.FormPublicFundAccountInfo");
            rm.addResource<wtPay.FormPublicFund.FormPublicFundCardType>("FormPublicFundCardType", "wtPay.FormPublicFund.FormPublicFundCardType");
            rm.addResource<wtPay.FormPublicFund.FormPublicFundCustomerInfo>("FormPublicFundCustomerInfo", "wtPay.FormPublicFund.FormPublicFundCustomerInfo");
            rm.addResource<wtPay.FormPublicFund.FormPublicFundDetailedInfo>("FormPublicFundDetailedInfo", "wtPay.FormPublicFund.FormPublicFundDetailedInfo");
            rm.addResource<wtPay.FormPublicFund.FormPublicFundInput>("FormPublicFundInput", "wtPay.FormPublicFund.FormPublicFundInput");
            rm.addResource<wtPay.FormPublicFund.FormPublicFundWait>("FormPublicFundWait", "wtPay.FormPublicFund.FormPublicFundWait");
            rm.addResource<wtPay.FormPublicFund.FormPublicLoanBalanceInfo>("FormPublicLoanBalanceInfo", "wtPay.FormPublicFund.FormPublicLoanBalanceInfo");
            rm.addResource<wtPay.FormPublicFund.FormPublicLoanDetailedInfo>("FormpublicLoanDetailedInfo", "wtPay.FormPublicFund.FormPublicLoanDetailedInfo");
            //社保
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecurity>("FormSocialSecurity", "wtPay.FormSocialSecurity.FormSocialSecurity");
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecurityInfo>("FormSocialSecurityInfo", "wtPay.FormSocialSecurity.FormSocialSecurityInfo");
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecurityInput>("FormSocialSecurityInput", "wtPay.FormSocialSecurity.FormSocialSecurityInput");
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecurityMedicalAccount>("FormSocialSecurityMedicalAccount", "wtPay.FormSocialSecurity.FormSocialSecurityMedicalAccount");
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecurityMedicalAccountConsume>("FormSocialSecurityMedicalAccountConsume", "wtPay.FormSocialSecurity.FormSocialSecurityMedicalAccountConsume");
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecurityPensionAccount>("FormSocialSecurityPensionAccount", "wtPay.FormSocialSecurity.FormSocialSecurityPensionAccount");
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecurityPensionGrant>("FormSocialSecurityPensionGrant", "wtPay.FormSocialSecurity.FormSocialSecurityPensionGrant");
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecurityWait>("FormSocialSecurityWait", "wtPay.FormSocialSecurity.FormSocialSecurityWait");
            rm.addResource<wtPay.FormSocialSecurity.FormSocialSecuritySelectTimet>("FormSocialSecuritySelectTimet", "wtPay.FormSocialSecurity.FormSocialSecuritySelectTimet");

            //维护人员页面  
            rm.addResource<wtPay.FormMaintainSign.FormMaintainSign>("FormMaintainSign", "wtPay.FormMaintainSign.FormMaintainSign");
            rm.addResource<wtPay.FormMaintainSign.FormMaintainSignWait>("FormMaintainSignWait", "wtPay.FormMaintainSign.FormMaintainSignWait");
            rm.addResource<wtPay.FormMaintainSign.FormMechineState>("FormMechineState", "wtPay.FormMaintainSign.FormMechineState");
            rm.addResource<wtPay.FormMaintainSign.FormNetTest>("FormNetTest", "wtPay.FormMaintainSign.FormNetTest");
            rm.addResource<wtPay.FormMaintainSign.FormRefund>("FormRefund", "wtPay.FormMaintainSign.FormRefund");
            rm.addResource<wtPay.FormMaintainSign.FormMechineTemp>("FormMechineTemp", "wtPay.FormMaintainSign.FormMechineTemp");

            //其它服务页面   
            rm.addResource<wtPay.GeneralForm.FormTemp>("FormTemp", "wtPay.GeneralForm.FormTemp");
            //挂号
            rm.addResource<wtPay.FormRegistration.FormRegistration>("FormRegistration", "wtPay.FormRegistration.FormRegistration");
            rm.addResource<wtPay.FormRegistration.FormRegistrationHospital_1>("FormRegistrationHospital_1", "wtPay.FormRegistration.FormRegistrationHospital_1");
            rm.addResource<wtPay.FormRegistration.FormRegistrationDepartment_2>("FormRegistrationDepartment_2", "wtPay.FormRegistration.FormRegistrationDepartment_2");
            rm.addResource<wtPay.FormRegistration.FormRegistrationDoctor_3>("FormRegistrationDoctor_3", "wtPay.FormRegistration.FormRegistrationDoctor_3");
            rm.addResource<wtPay.FormRegistration.FormRegistrationInput>("FormRegistrationInput", "wtPay.FormRegistration.FormRegistrationInput");
            rm.addResource<wtPay.FormRegistration.FormRegistrationWait>("FormRegistrationWait", "wtPay.FormRegistration.FormRegistrationWait");
            rm.addResource<wtPay.FormRegistration.FormRegistration_success>("FormRegistration_success", "wtPay.FormRegistration.FormRegistration_success");
            rm.addResource<wtPay.FormRegistration.FormRegistrationUndoInput>("FormRegistrationUndoInput", "wtPay.FormRegistration.FormRegistrationUndoInput");
            rm.addResource<wtPay.FormRegistration.FormRegistrationRecord>("FormRegistrationRecord", "wtPay.FormRegistration.FormRegistrationRecord");
            rm.addResource<wtPay.FormRegistration.FormRegistrationSw>("FormRegistrationSw", "wtPay.FormRegistration.FormRegistrationSw");
            //水务   
            rm.addResource<wtPay.FormWater.FormWaterStep01>("FormWaterStep01", "wtPay.FormWater.FormWaterStep01");
            rm.addResource<wtPay.FormWater.FormWaterStep02>("FormWaterStep02", "wtPay.FormWater.FormWaterStep02");
            rm.addResource<wtPay.FormWater.FormWaterStep03>("FormWaterStep03", "wtPay.FormWater.FormWaterStep03");
            rm.addResource<wtPay.FormWater.FormWaterStep06>("FormWaterStep06", "wtPay.FormWater.FormWaterStep06");
            rm.addResource<wtPay.FormWater.FormWaterStep06_success>("FormWaterStep06_success", "wtPay.FormWater.FormWaterStep06_success");

            //广电
            rm.addResource<wtPay.FormBroadCas.FormBroadCasStep01>("FormBroadCasStep01", "wtPay.FormBroadCas.FormBroadCasStep01");
            rm.addResource<wtPay.FormBroadCas.FormBroadCasStep02>("FormBroadCasStep02", "wtPay.FormBroadCas.FormBroadCasStep02");
            rm.addResource<wtPay.FormBroadCas.FormBroadCasStep03>("FormBroadCasStep03", "wtPay.FormBroadCas.FormBroadCasStep03");
            rm.addResource<wtPay.FormBroadCas.FormBroadCasStep06>("FormBroadCasStep06", "wtPay.FormBroadCas.FormBroadCasStep06");
            rm.addResource<wtPay.FormBroadCas.FormBroadCasStep06_success>("FormBroadCasStep06_success", "wtPay.FormBroadCas.FormBroadCasStep06_success");

            //热力
            rm.addResource<wtPay.FormHeat.FormHeatStep01>("FormHeatStep01", "wtPay.FormHeat.FormHeatStep01");
            rm.addResource<wtPay.FormHeat.FormHeatStep02>("FormHeatStep02", "wtPay.FormHeat.FormHeatStep02");
            rm.addResource<wtPay.FormHeat.FormHeatStep06>("FormHeatStep06", "wtPay.FormHeat.FormHeatStep06");
            rm.addResource<wtPay.FormHeat.FormHeatStep06_success>("FormHeatStep06_success", "wtPay.FormHeat.FormHeatStep06_success");

            //物业
            rm.addResource<wtPay.FormProp.FormPropStep>("FormPropStep", "wtPay.FormProp.FormPropStep");
            rm.addResource<wtPay.FormProp.FormPropStep01>("FormPropStep01", "wtPay.FormProp.FormPropStep01");
            rm.addResource<wtPay.FormProp.FormPropStep02>("FormPropStep02", "wtPay.FormProp.FormPropStep02");
            rm.addResource<wtPay.FormProp.FormPropStep02_house>("FormPropStep02_house", "wtPay.FormProp.FormPropStep02_house");
            rm.addResource<wtPay.FormProp.FormPropStep02_ParkingLot>("FormPropStep02_ParkingLot", "wtPay.FormProp.FormPropStep02_ParkingLot");
            rm.addResource<wtPay.FormProp.FormPropStep06>("FormPropStep06", "wtPay.FormProp.FormPropStep06");
            rm.addResource<wtPay.FormProp.FormPropStep06_success>("FormPropStep06_success", "wtPay.FormProp.FormPropStep06_success");
            //小区物业
            rm.addResource<wtPay.FormProp.FormPropStepInCard01>("FormPropStepInCard01", "wtPay.FormProp.FormPropStepInCard01");
            rm.addResource<wtPay.FormProp.FormPropStepTemp02>("FormPropStepTemp02", "wtPay.FormProp.FormPropStepTemp02");
            rm.addResource<wtPay.FormProp.FormPropStepTemp03>("FormPropStepTemp03", "wtPay.FormProp.FormPropStepTemp03");
            rm.addResource<wtPay.FormProp.FormPropStepTemp06>("FormPropStepTemp06", "wtPay.FormProp.FormPropStepTemp06");
            rm.addResource<wtPay.FormProp.FormPropStepTemp06_success>("FormPropStepTemp06_success", "wtPay.FormProp.FormPropStepTemp06_success");

            //快递查询
            rm.addResource<wtPay.FormExpress.FormExpressType>("FormExpressType", "wtPay.FormExpress.FormExpressType");
            rm.addResource<wtPay.FormExpress.FormExpressResult>("FormExpressResult", "wtPay.FormExpress.FormExpressResult");
            rm.addResource<wtPay.FormExpress.FormExpressInput>("FormExpressInput", "wtPay.FormExpress.FormExpressInput");

            //交通违章 
            rm.addResource<wtPay.FormTraffic.FormTrafficViolation>("FormTrafficViolation", "wtPay.FormTraffic.FormTrafficViolation");

            //新闻 FormNewsDetails
            rm.addResource<wtPay.FormNews.FormNewsDetails>("FormNewsDetails", "wtPay.FormNews.FormNewsDetails");
            rm.addResource<wtPay.FormNews.FormNewsList>("FormNewsList", "wtPay.FormNews.FormNewsList");
            //物业二次专供
            rm.addResource<wtPay.FormPropSec.FormPropSec01>("FormPropSec01", "wtPay.FormPropSec.FormPropSec01");
            rm.addResource<wtPay.FormPropSec.FormPropSec01_2>("FormPropSec01_2", "wtPay.FormPropSec.FormPropSec01_2");
            rm.addResource<wtPay.FormPropSec.FormPropSecStep02>("FormPropSecStep02", "wtPay.FormPropSec.FormPropSecStep02");
            rm.addResource<wtPay.FormPropSec.FormPropSecStep03>("FormPropSecStep03", "wtPay.FormPropSec.FormPropSecStep03");
            rm.addResource<wtPay.FormPropSec.FormPropSecStep04>("FormPropSecStep04", "wtPay.FormPropSec.FormPropSecStep04");
            rm.addResource<wtPay.FormPropSec.FormPropSecStep07>("FormPropSecStep07", "wtPay.FormPropSec.FormPropSecStep07");
            rm.addResource<wtPay.FormPropSec.FormPropSecStep08_success>("FormPropSecStep08_success", "wtPay.FormPropSec.FormPropSecStep08_success");
            rm.addResource<wtPay.FormPropSec.FormPropSecStep09>("FormPropSecStep09", "wtPay.FormPropSec.FormPropSecStep09");
        }
    }
}
