INCLUDE globals.ink

Tamira is standing near the edge of the basin when you approach.

She watches the treeline as if expecting something to emerge from it.

Tamira: You're awake.
Tamira: Good. I was beginning to wonder if the fall had finished you.
Tamira: The sky doesn't treat travelers kindly here.

-> main_hub


=== main_hub
~register_npc(NPC.tamira)
+ [I should probably keep moving.]
    -> done

+ [You saw my ship crash?]
    Tamira nods slowly.
    Tamira: Hard to miss something like that.
    Tamira: The forest carried the sound for miles.
    Tamira: A bright streak through the clouds. Then silence.
    Tamira: You're not the first to arrive that way.
    -> crash_hub

+ [Where exactly am I?]
    Tamira gestures to the surrounding forest.

    Tamira: Echo Basin.
    Tamira: Quiet place. Isolated.
    Tamira: Most travelers start here, whether they intend to or not.
    -> where_hub

+ [Why do you stay here alone?]
    Tamira shrugs slightly.

    Tamira: Someone has to watch the roads.
    Tamira: Travelers pass through here more often than you might think.
    Tamira: Some are explorers like you.
    Tamira: Others are scavengers.
    Tamira: Most are just trying to figure out what to do next.
    -> alone_hub


=== crash_hub
+ [I should probably keep moving.]
    -> done

+ [Ships crash here often?]
    Tamira: Often enough that the sky no longer surprises anyone.
    Tamira: Some say it's bad navigation.
    Tamira: Others say the storms are unpredictable.

    Tamira looks back toward the sky.

    Tamira: But after seeing it happen enough times...
    Tamira: ...you begin to wonder if something else is involved.
    -> crash_hub

+ [So what do you think it is?]
    Tamira: If I had a clean answer, I wouldn't still be watching the sky.
    -> crash_hub

+ [What happens to the people who survive?]
    Tamira: Some move on quickly.
    Tamira: Some go looking for answers.
    Tamira: Some disappear into the wilds and are never seen again.
    -> crash_hub


=== where_hub
+ [I should probably keep moving.]
    -> done

+ [Are there any settlements nearby?]
    Tamira: A few.

    Tamira draws two simple lines in the dirt with a stick.

    Tamira: If you travel far enough east, you'll find the scholars.
    Tamira: They built a place called Luminar Outpost.
    Tamira: They're trying to recover what the old world lost.

    Tamira pauses.

    Tamira: Head the other direction and the land climbs into the mountains.
    Tamira: That's where the revivalists live.
    Tamira: They believe the collapse happened for a reason.
    Tamira: They prefer not to wake the past.
    -> where_hub

+ [And you? Where do you fit?]
    Tamira: I fit where the road is.
    -> where_hub

+ [Why do travelers end up here?]
    Tamira: That is the question, isn't it?
    Tamira: Some call it luck.
    Tamira: Some call it design.
    -> final_hub


=== alone_hub
+ [I should probably keep moving.]
    -> done

+ [And what do you think I should do?]
    Tamira studies you for a moment.

    Tamira: That depends on what kind of traveler you are.
    Tamira: Some people arrive here and immediately start looking for a way home.
    Tamira: Others get curious.
    Tamira: Aurelia has a way of making people curious.
    -> alone_hub

+ [Does watching people pass through ever get old?]
    Tamira: No.
    Tamira: New faces. Same questions. Different endings.
    -> alone_hub

+ [So what kind of traveler do you think I am?]
    Tamira looks at you for a long moment.
    Tamira: That depends on which question you ask next.
    -> final_hub


=== final_hub
+ [Thanks for all that. I should get going.]
    -> done

+ [If I wanted answers, where would I go?]
    Tamira: Find the archivist.
    Tamira: They keep to the old writings not far from here.
    Tamira: If anyone can help you understand this world, it's them.
    -> final_hub

+ [If I wanted to repair my ship, where would I go?]
    Tamira: Find the scrappers.
    Tamira: They know how to pull use from broken things.
    Tamira: If your ship can be repaired, they'll know where to start.
    -> final_hub

+ [What should I be most careful of out here?]
    Tamira: Certainty.
    Tamira: Storms, ruins, and wild things can all be survived.
    Tamira: It's the people who think they already understand Aurelia who get lost fastest.
    -> final_hub


=== done
Tamira nods.

Tamira: You probably should.
Tamira: If you're trying to repair your ship, you'll want to speak with the scrappers.
Tamira: And if you want answers about this world...
Tamira: The archivist studies the old writings not far from here.
Tamira: Either way, traveler...
Tamira: Welcome to Aurelia.

-> END