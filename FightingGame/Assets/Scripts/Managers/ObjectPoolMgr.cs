using FGDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> poolStack = new Stack<Poolable>(); 

        public void Init(GameObject original, int poolCount)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";
        
            for(int i = 0; i < poolCount; i++)
                Push(Create());
        }

        public void Add(GameObject original, int poolCount)
        {
            if (Original != original)
            {
                Debug.Log($"Failed to Add : {original}");
                return;
            }

            for (int i = 0; i < poolCount; i++)
                Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;
            poolable.Init();

            poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();
            
            poolable.gameObject.SetActive(true);
            poolable.transform.parent = parent;
            poolable.isUsing = true;

            return poolable;
        }

        public Poolable Pop(bool active)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            if(active)
                poolable.gameObject.SetActive(true);
            
            poolable.isUsing = true;

            return poolable;
        }

        public Poolable Pop(Vector2 position)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.position = position;
            poolable.isUsing = true;

            return poolable;
        }

        public Poolable Pop(Vector2 position, Quaternion rotation, bool active = false)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(active);
            poolable.transform.position = position;
            poolable.transform.rotation = rotation;
            poolable.isUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
    Transform root;

    const int POOLCOUNT = 3;

    public void Init()
    {
        if (root == null)
        {
            root = new GameObject { name = "@Pool_Root" }.transform;
        }

        PhotonPrefabPool.Init();
    }

    public void CreatePool(GameObject original, int count)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = root;

        pools.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if(pools.ContainsKey(name) == false) // 이 경우엔 파괴
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        pools[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (pools.ContainsKey(original.name) == false)
            CreatePool(original, POOLCOUNT); // poolCount만큼 생성

        return pools[original.name].Pop(parent);
    }

    public Poolable Pop(GameObject original, bool active = true)
    {
        if (pools.ContainsKey(original.name) == false)
            CreatePool(original, POOLCOUNT); // poolCount만큼 생성

        return pools[original.name].Pop(active);
    }

    public Poolable Pop(GameObject original, Vector2 position)
    {
        if (pools.ContainsKey(original.name) == false)
            CreatePool(original, POOLCOUNT); // poolCount만큼 생성

        return pools[original.name].Pop(position);
    }

    public Poolable Pop(GameObject original, Vector2 position, Quaternion rotation, bool active = false)
    {
        if (pools.ContainsKey(original.name) == false)
            CreatePool(original, POOLCOUNT); // poolCount만큼 생성

        return pools[original.name].Pop(position, rotation, active);
    }

    public void GeneratePool(GameObject original, int count, Transform parent = null)
    {
        if (pools.ContainsKey(original.name) == true)
        {
            pools.TryGetValue(original.name, out var _pool);
            _pool.Add(original, count);
            return;
        }

        CreatePool(original, count);
    }

    public GameObject GetOriginal(string name)
    {
        if (pools.ContainsKey(name) == false)
            return null;

        return pools[name].Original;
    }

    #region CharacterPool
    private bool isCharacterCommonPoolState = false;
    public bool[] isCharacterPoolStates = new bool[(int)ENUM_CHARACTER_TYPE.Max];

    public void GenerateCharacterPoolAll()
    {
        for (int i = 1; i < (int)ENUM_CHARACTER_TYPE.Max; i++)
            GenerateCharacterEffectPool((ENUM_CHARACTER_TYPE)i);
    }

    public void GenerateCharacterEffectPool(ENUM_CHARACTER_TYPE characterType)
    {
        if (isCharacterCommonPoolState == false)
        {
            isCharacterCommonPoolState = true;
            GenerateCommonEffectPool();
        }

        if (isCharacterPoolStates[(int)characterType] == false)
        {
            isCharacterPoolStates[(int)characterType] = true;

            switch (characterType)
            {
                case ENUM_CHARACTER_TYPE.Knight:
                    GenerateKnightEffectPool();
                    break;
                case ENUM_CHARACTER_TYPE.Wizard:
                    GenerateWizardEffectPool();
                    break;
                default:
                    Debug.LogWarning($"없는 캐릭터 : {characterType}");
                    break;
            }
        }
    }

    private static void GenerateCommonEffectPool()
    {
        // Common Effect
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_AttackedEffect1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_AttackedEffect2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_AttackedEffect3}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_SkillAttackedEffect1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_SkillAttackedEffect2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_SkillAttackedEffect3}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_SkillAttackedEffect4}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_SkillAttackedEffect5}", POOLCOUNT);
    }
    private static void GenerateKnightEffectPool()
    {
        // Attack
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_Attack1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_Attack2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_Attack3}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_DashSkill_1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_DashSkill_2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_DashSkill_3}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_JumpAttack}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ThrowSkillObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_SmashSkillObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_SmashSkillObject_1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_SmashSkillObject_2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_SmashSkillObject_3}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_3}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_4}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_5}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_DashSkillObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkillObject}", POOLCOUNT);

        // Effect
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_JumpUp}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_Landing}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_Move}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_Dash}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_DashSkill}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_ComboSkill1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_ComboSkill2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_ComboSkill3}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_ComboSkill4}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_ComboSkill5}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_ComboSkill6}", POOLCOUNT);
    }
    private static void GenerateWizardEffectPool()
    {
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_Attack1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_Attack2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThrowAttackObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThrowJumpAttackObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThunderSkillObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThunderSkillObject_1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThunderSkillObject_2}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThunderSkillObject_3}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_IceSkillObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_IceSkillObject_1}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_MeteorSkillObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_MeteorSkillObject_Falling}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_MeteorSkillObject_Explode}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_PushOutSkillObject}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_PushOutSkillObject_1}", POOLCOUNT);

        // Effect
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_ThunderCircleEffect}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_MagicalJinEffect}", 10); // Particle
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_StarlightEffect}", 10); // Particle
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_DashEffect}", POOLCOUNT);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_FlameEffect}", POOLCOUNT);
    }
    #endregion // << CharacterPool

    public void Clear()
    {
        if (root != null)
        {
            foreach (Transform child in root)
                GameObject.Destroy(child.gameObject);
        }

        pools.Clear();

        isCharacterCommonPoolState = false;
        for (int i = 0; i < isCharacterPoolStates.Length; i++)
            isCharacterPoolStates[i] = false;
    }
}
