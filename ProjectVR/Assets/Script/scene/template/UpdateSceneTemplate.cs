﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class UpdateSceneTemplate : Singleton<UpdateSceneTemplate> {

    // ※消さないでください
    void Awake()
    {
        base.Awake();
    }

    // ※消さないでください
    void OnEnable()
    {
    }

	/**
     *  初期化処理です
     */
	void Start () {
		// 初期化処理をここに書きます
        // 初期化処理なので実行されるのはシーン開始時の1回だけです
	}
	
	/**
     * 毎フレームの更新処理です
     */
	void Update () {
		// 毎フレームの更新処理をここに書きます
	}
}
