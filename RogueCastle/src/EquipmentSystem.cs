using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;

namespace RogueCastle
{
    public class EquipmentSystem
    {
        private List<EquipmentData[]> m_equipmentDataArray;
        private List<int[]> m_abilityCostArray;
        private float AbilityCostMod = 175;//200;//175; //200;
        private int AbilityCostBase = 175;//200;//175; //200;

        public EquipmentSystem()
        {
            m_equipmentDataArray = new List<EquipmentData[]>();
            for (int i = 0; i < EquipmentCategoryType.TOTAL; i++)
            {
                EquipmentData[] dataArray = new EquipmentData[EquipmentBaseType.TOTAL];
                for (int j = 0; j < EquipmentBaseType.TOTAL; j++)
                    dataArray[j] = new EquipmentData();
                m_equipmentDataArray.Add(dataArray);
            }

            m_abilityCostArray = new List<int[]>();
            for (int i = 0; i < EquipmentCategoryType.TOTAL; i++)
            {
                int[] dataArray = new int[EquipmentAbilityType.TOTAL];
                for (int j = 0; j < EquipmentAbilityType.TOTAL; j++)
                    dataArray[j] = 9999;
                m_abilityCostArray.Add(dataArray);
            }
        }

        public void InitializeAbilityCosts()
        {
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.DOUBLE_JUMP] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.DASH] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.VAMPIRISM] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.FLIGHT] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.MANA_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.MOVEMENT_SPEED] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.DAMAGE_RETURN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.GOLD_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.MANA_HP_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.ROOM_LEVEL_DOWN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.SWORD][EquipmentAbilityType.ROOM_LEVEL_UP] = AbilityCostBase;

            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.DOUBLE_JUMP] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.DASH] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.VAMPIRISM] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.FLIGHT] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.MANA_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.MOVEMENT_SPEED] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.DAMAGE_RETURN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.GOLD_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.MANA_HP_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.ROOM_LEVEL_DOWN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CAPE][EquipmentAbilityType.ROOM_LEVEL_UP] = AbilityCostBase;

            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.DOUBLE_JUMP] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.DASH] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.VAMPIRISM] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.FLIGHT] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.MANA_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.MOVEMENT_SPEED] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.DAMAGE_RETURN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.GOLD_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.MANA_HP_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.ROOM_LEVEL_DOWN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.CHEST][EquipmentAbilityType.ROOM_LEVEL_UP] = AbilityCostBase;

            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.DOUBLE_JUMP] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.DASH] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.VAMPIRISM] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.FLIGHT] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.MANA_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.MOVEMENT_SPEED] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.DAMAGE_RETURN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.GOLD_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.MANA_HP_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.ROOM_LEVEL_DOWN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.LIMBS][EquipmentAbilityType.ROOM_LEVEL_UP] = AbilityCostBase;

            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.DOUBLE_JUMP] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.DASH] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.VAMPIRISM] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.FLIGHT] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.MANA_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.MOVEMENT_SPEED] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.DAMAGE_RETURN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.GOLD_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.MANA_HP_GAIN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.ROOM_LEVEL_DOWN] = AbilityCostBase;
            m_abilityCostArray[EquipmentCategoryType.HELM][EquipmentAbilityType.ROOM_LEVEL_UP] = AbilityCostBase;


        }

        public void InitializeEquipmentData()
        {
            //Equipment Type, Equipment Index, Health, Mana, Damage, Weight, Color1, Color2, Cost, Secondary Attributes.
            //CreateEquipmentData(EquipmentCategoryType.Sword, 0, 10, 0.1f, 0, 10, 5, Color.Green, Color.Blue, 100, new Vector2(EquipmentSecondaryDataType.CritDamage, 1), new Vector2(EquipmentSecondaryDataType.CritDamage, 10));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 1, 0, 0, 250, 15, 0, 7, 0, 0, 0, new Color(255, 230, 175), new Color(70, 130, 255));
            CreateEquipmentData(EquipmentCategoryType.HELM, 1, 0, 0, 150, 10, 20, 0, 0, 5, 0, new Color(255, 221, 130), new Color(70, 130, 255));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 3, 2, 0, 300, 10, 0, 0, 0, 0, 0, new Color(255, 221, 130), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.CRIT_CHANCE, 0.04f));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 1, 0, 0, 200, 10, 0, 0, 0, 0, 5, new Color(210, 195, 125), new Color(210, 195, 125));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 1, 0, 0, 350, 20, 0, 0, 9, 0, 0, new Color(190, 165, 105), new Color(155, 190, 366));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 5, 0, 5, 375, 20, 0, 11, 0, 0, 0, new Color(135, 135, 135), new Color(95, 95, 95));
            CreateEquipmentData(EquipmentCategoryType.HELM, 3, 0, 5, 250, 15, 25, 0, 0, 20, 0, new Color(165, 165, 165), new Color(125, 235, 80));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 5, 0, 5, 425, 15, 0, 0, 0, 0, 0, new Color(125, 235, 80), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.CRIT_DAMAGE, 0.15f));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 3, 0, 5, 375, 20, 0, 0, 0, 0, 11, new Color(155, 155, 155), new Color(155, 155, 155));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 3, 0, 5, 525, 30, 0, 0, 16, 0, 0, new Color(120, 120, 120), new Color(95, 95, 95));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 1, 2, 10, 500, 35, -30, 9, 0, 0, 0, new Color(105, 0, 0), new Color(105, 0, 0), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1));
            CreateEquipmentData(EquipmentCategoryType.HELM, 1, 3, 10, 500, 35, -30, 0, 0, 25, 0, new Color(230, 0, 0), new Color(105, 0, 0), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 1, 2, 10, 500, 35, -30, 0, 0, 0, 0, new Color(230, 0, 0), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 1, 3, 10, 500, 35, -30, 0, 0, 0, 9, new Color(230, 0, 0), new Color(105, 0, 0), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 1, 2, 10, 500, 35, -30, 0, 14, 0, 0, new Color(230, 0, 0), new Color(105, 0, 0), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 10, 2, 1, 750, 35, 0, 17, 0, 0, 0, new Color(245, 255, 175), new Color(215, 215, 215));
            CreateEquipmentData(EquipmentCategoryType.HELM, 8, 0, 1, 550, 25, 35, 0, 0, 25, 0, new Color(245, 255, 175), new Color(215, 215, 215));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 11, 2, 1, 850, 35, 0, 0, 0, 0, 0, new Color(245, 255, 175), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 8, 0, 1, 750, 35, 0, 0, 0, 0, 17, new Color(245, 255, 175), new Color(245, 255, 175));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 8, 0, 1, 950, 45, 0, 0, 25, 0, 0, new Color(245, 255, 175), new Color(215, 215, 215));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 15, 2, 6, 1150, 40, 0, 22, 0, 0, 0, new Color(10, 150, 50), new Color(10, 150, 50));
            CreateEquipmentData(EquipmentCategoryType.HELM, 12, 0, 6, 950, 30, 55, 0, 0, 15, 0, new Color(180, 120, 70), new Color(78, 181, 80));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 13, 2, 6, 1300, 40, 0, 0, 0, 0, 0, new Color(135, 200, 130), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.CRIT_CHANCE, 0.04f), new Vector2(EquipmentSecondaryDataType.CRIT_DAMAGE, 0.15f));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 12, 0, 6, 1075, 40, 0, 0, 0, 0, 22, new Color(160, 125, 80), new Color(160, 125, 80));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 12, 0, 6, 1500, 55, 0, 0, 38, 0, 0, new Color(180, 120, 70), new Color(145, 55, 15));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 10, 2, 11, 1250, 45, 0, -10, 0, 0, 0, new Color(240, 235, 90), new Color(215, 135, 75), new Vector2(EquipmentSecondaryDataType.GOLD_BONUS, 0.1f));
            CreateEquipmentData(EquipmentCategoryType.HELM, 10, 3, 11, 1250, 45, 0, -10, 0, 0, 0, new Color(240, 235, 90), new Color(215, 135, 75), new Vector2(EquipmentSecondaryDataType.GOLD_BONUS, 0.1f));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 10, 3, 11, 1250, 75, 0, 0, 0, 0, 0, new Color(210, 240, 75), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.GOLD_BONUS, 0.1f));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 10, 2, 11, 1250, 45, 0, -10, 0, 0, 0, new Color(235, 220, 135), new Color(235, 220, 135), new Vector2(EquipmentSecondaryDataType.GOLD_BONUS, 0.1f));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 10, 3, 11, 1250, 45, 0, -10, 0, 0, 0, new Color(240, 235, 90), new Color(235, 135, 75), new Vector2(EquipmentSecondaryDataType.GOLD_BONUS, 0.1f));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 18, 0, 2, 1450, 55, 0, 14, 12, 0, 0, new Color(255, 190, 45), new Color(240, 230, 0));
            CreateEquipmentData(EquipmentCategoryType.HELM, 15, 0, 2, 1200, 45, 20, 0, 18, 20, 0, new Color(255, 190, 45), new Color(240, 230, 0));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 19, 2, 2, 1600, 40, 0, 0, 27, 0, 0, new Color(255, 190, 45), new Color(0, 0, 0));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 15, 0, 2, 1400, 45, 0, 0, 18, 0, 10, new Color(255, 190, 45), new Color(240, 230, 0));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 15, 3, 2, 1875, 80, 0, 0, 70, 0, 0, new Color(255, 190, 45), new Color(240, 230, 0));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 21, 2, 7, 1700, 45, 0, 27, 0, 0, 0, new Color(170, 255, 250), new Color(255, 245, 255));
            CreateEquipmentData(EquipmentCategoryType.HELM, 18, 0, 7, 1500, 40, 40, 0, 0, 45, 0, new Color(115, 175, 185), new Color(255, 245, 255));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 23, 2, 7, 2100, 55, 0, 0, 0, 0, 0, new Color(170, 255, 250), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.DOUBLE_JUMP, 2));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 18, 0, 7, 1800, 50, 0, 0, 0, 0, 32, new Color(255, 245, 255), new Color(255, 245, 255));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 18, 0, 7, 1650, 65, 0, 0, 48, 0, 0, new Color(115, 175, 185), new Color(170, 255, 250));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 20, 3, 12, 1600, 50, 0, 15, -15, 0, 0, new Color(60, 60, 60), new Color(30, 30, 30), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.HELM, 20, 3, 12, 1400, 50, 30, 0, -15, 30, 0, new Color(90, 90, 90), new Color(30, 30, 30), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 20, 2, 12, 1800, 35, 0, 0, -15, 0, 0, new Color(85, 15, 5), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 20, 3, 12, 1500, 50, 0, 0, -15, 0, 15, new Color(30, 30, 30), new Color(90, 90, 90), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 20, 2, 12, 1850, 50, 30, 0, -15, 30, 0, new Color(90, 90, 90), new Color(30, 30, 30), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 27, 2, 3, 2300, 55, 0, 36, 0, 0, 0, new Color(240, 230, 0), new Color(240, 230, 0));
            CreateEquipmentData(EquipmentCategoryType.HELM, 25, 0, 3, 1900, 50, 55, 0, 0, 40, 0, new Color(115, 100, 190), new Color(240, 230, 0));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 29, 2, 3, 2600, 55, 0, 0, 0, 0, 0, new Color(55, 0, 145), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.CRIT_DAMAGE, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 25, 0, 3, 2250, 55, 0, 0, 0, 0, 36, new Color(115, 100, 190), new Color(50, 55, 210));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 25, 2, 3, 3000, 75, 0, 0, 59, 0, 0, new Color(115, 100, 190), new Color(50, 55, 210));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 31, 2, 8, 2800, 75, 0, 46, 0, 0, 0, new Color(200, 0, 0), new Color(50, 50, 50));
            CreateEquipmentData(EquipmentCategoryType.HELM, 29, 0, 8, 2400, 60, 75, 0, 0, 60, 0, new Color(50, 50, 50), new Color(120, 0, 0));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 34, 3, 8, 3200, 75, 0, 0, 0, 0, 0, new Color(255, 0, 0), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 2));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 29, 0, 8, 2600, 60, 0, 0, 0, 0, 42, new Color(50, 50, 50), new Color(50, 50, 50));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 29, 2, 8, 3750, 85, 0, 0, 78, 0, 0, new Color(160, 0, 0), new Color(80, 0, 0));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 30, 3, 13, 3200, 55, 0, 20, 0, 25, 0, new Color(215, 245, 70), new Color(255, 255, 255), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1));
            CreateEquipmentData(EquipmentCategoryType.HELM, 30, 3, 13, 2800, 55, 25, 0, 0, 50, 0, new Color(215, 245, 70), new Color(255, 255, 255), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 30, 3, 13, 3300, 65, 0, 0, 0, 25, 0, new Color(215, 245, 70), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 30, 3, 13, 3000, 100, 0, 0, 0, 25, 72, new Color(215, 245, 70), new Color(255, 255, 255), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 30, 2, 13, 3800, 65, 0, 0, 40, 25, 0, new Color(215, 245, 70), new Color(255, 255, 255), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 40, 3, 4, 4250, 85, 0, 60, 0, 0, 0, new Color(20, 60, 255), new Color(255, 40, 40));
            CreateEquipmentData(EquipmentCategoryType.HELM, 38, 0, 4, 3750, 70, 90, 0, 0, 90, 0, new Color(180, 180, 180), new Color(110, 100, 240));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 43, 2, 4, 4100, 80, 0, 0, 0, 0, 0, new Color(255, 255, 255), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.FLIGHT, 3));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 38, 0, 4, 3800, 85, 0, 0, 0, 0, 60, new Color(150, 150, 150), new Color(120, 120, 120));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 38, 2, 4, 4500, 125, 0, 0, 100, 0, 0, new Color(120, 120, 120), new Color(105, 80, 240));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 52, 2, 9, 4750, 75, 0, 40, 0, 0, 0, new Color(125, 125, 125), new Color(60, 130, 70), new Vector2(EquipmentSecondaryDataType.CRIT_CHANCE, 0.04f), new Vector2(EquipmentSecondaryDataType.CRIT_DAMAGE, 0.15f), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.HELM, 49, 2, 9, 3900, 65, 40, 0, 0, 40, 0, new Color(255, 255, 255), new Color(70, 70, 70), new Vector2(EquipmentSecondaryDataType.CRIT_CHANCE, 0.04f), new Vector2(EquipmentSecondaryDataType.CRIT_DAMAGE, 0.15f), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 57, 2, 9, 5750, 85, 0, 0, 0, 0, 0, new Color(255, 250, 120), new Color(0, 0, 0), new Vector2(EquipmentSecondaryDataType.CRIT_CHANCE, 0.04f), new Vector2(EquipmentSecondaryDataType.CRIT_DAMAGE, 0.15f), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 46, 2, 9, 4600, 60, 0, 0, 0, 0, 40, new Color(255, 255, 255), new Color(70, 70, 70), new Vector2(EquipmentSecondaryDataType.CRIT_CHANCE, 0.04f), new Vector2(EquipmentSecondaryDataType.CRIT_DAMAGE, 0.15f), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 46, 2, 9, 6750, 140, 0, 0, 50, 0, 0, new Color(255, 255, 255), new Color(70, 70, 70), new Vector2(EquipmentSecondaryDataType.CRIT_CHANCE, 0.04f), new Vector2(EquipmentSecondaryDataType.CRIT_DAMAGE, 0.15f), new Vector2(EquipmentSecondaryDataType.DAMAGE_RETURN, 0.5f));
            CreateEquipmentData(EquipmentCategoryType.SWORD, 50, 3, 14, 6000, 100, 0, 50, 0, 0, 0, new Color(15, 15, 15), new Color(15, 15, 15), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1), new Vector2(EquipmentSecondaryDataType.AIR_DASH, 1));
            CreateEquipmentData(EquipmentCategoryType.HELM, 50, 3, 14, 6000, 100, 60, 0, 0, 60, 0, new Color(45, 45, 45), new Color(15, 15, 15), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1), new Vector2(EquipmentSecondaryDataType.DOUBLE_JUMP, 1));
            CreateEquipmentData(EquipmentCategoryType.CAPE, 50, 3, 14, 6000, 100, 0, 0, 0, 0, 0, new Color(15, 15, 15), new Color(15, 15, 15), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1), new Vector2(EquipmentSecondaryDataType.DOUBLE_JUMP, 1));
            CreateEquipmentData(EquipmentCategoryType.LIMBS, 50, 3, 14, 6000, 100, 0, 0, 0, 0, 50, new Color(45, 45, 45), new Color(45, 45, 45), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1), new Vector2(EquipmentSecondaryDataType.DOUBLE_JUMP, 1));
            CreateEquipmentData(EquipmentCategoryType.CHEST, 50, 3, 14, 6000, 100, 50, 0, 0, 0, 0, new Color(15, 15, 15), new Color(15, 15, 15), new Vector2(EquipmentSecondaryDataType.VAMPIRISM, 1), new Vector2(EquipmentSecondaryDataType.MANA_DRAIN, 1), new Vector2(EquipmentSecondaryDataType.AIR_DASH, 1));
        }

        //private void CreateEquipmentData(int equipmentType, int equipmentIndex, int bonusHealth, float dmgReduc, int bonusMana, int bonusDamage, int weight, Color firstColour, Color secondColour, int cost, params Vector2[] secondaryAttributes)
        private void CreateEquipmentData(int equipmentType, byte levelRequirement, byte chestColourRequirement, int equipmentIndex, int cost, int weight, int bonusHealth, int bonusDamage, int BonusArmor, int bonusMana, int bonusMagic, Color firstColour, Color secondColour, params Vector2[] secondaryAttributes)
        {
            EquipmentData data = new EquipmentData();
            data.BonusDamage = bonusDamage;
            data.BonusHealth = bonusHealth;
            data.BonusArmor = BonusArmor;
            data.BonusMana = bonusMana;
            data.BonusMagic = bonusMagic;
            data.Weight = weight;
            data.FirstColour = firstColour;
            data.SecondColour = secondColour;
            data.Cost = cost;
            data.LevelRequirement = levelRequirement;
            data.ChestColourRequirement = chestColourRequirement;
            data.SecondaryAttribute = new Vector2[secondaryAttributes.Length];
            for (int i = 0; i < secondaryAttributes.Length; i++)
            {
                data.SecondaryAttribute[i] = secondaryAttributes[i];
            }

            m_equipmentDataArray[equipmentType][equipmentIndex] = data;
        }

        public EquipmentData GetEquipmentData(int categoryType, int equipmentIndex)
        {
            return m_equipmentDataArray[categoryType][equipmentIndex];
        }

        public int GetAbilityCost(int categoryType, int itemIndex)
        {
            return (int)(m_abilityCostArray[categoryType][itemIndex] + (Game.PlayerStats.TotalRunesPurchased * AbilityCostMod));
        }

        public int GetBaseAbilityCost(int categoryType, int itemIndex)
        {
            return (m_abilityCostArray[categoryType][itemIndex]);
        }

        public void SetBlueprintState(byte state)
        {
            foreach (byte[] stateArray in Game.PlayerStats.GetBlueprintArray)
            {
                for (int i = 0; i < stateArray.Length; i++)
                {
                    if (stateArray[i] < state)
                        stateArray[i] = state;
                }
            }

            foreach (byte[] stateArray in Game.PlayerStats.GetRuneArray)
            {
                for (int i = 0; i < stateArray.Length; i++)
                {
                    if (stateArray[i] < state)
                        stateArray[i] = state;
                }
            }
        }
    }
}
